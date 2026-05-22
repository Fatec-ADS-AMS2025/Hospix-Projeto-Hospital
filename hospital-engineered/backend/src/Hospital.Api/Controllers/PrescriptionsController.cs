using Hospital.Application.DTOs;
using Hospital.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController : HospitalControllerBase
{
    private readonly CreatePrescriptionUseCase createPrescription;
    private readonly PrescriptionUseCases prescriptions;

    public PrescriptionsController(CreatePrescriptionUseCase createPrescription, PrescriptionUseCases prescriptions)
    {
        this.createPrescription = createPrescription;
        this.prescriptions = prescriptions;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PrescriptionDto>>> List(CancellationToken cancellationToken)
    {
        return Ok(await prescriptions.ListAsync(cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<PrescriptionDto>> Create(CreatePrescriptionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await createPrescription.ExecuteAsync(request, cancellationToken));
        }
        catch (Exception ex)
        {
            return ApiError(ex);
        }
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<ActionResult<PrescriptionDto>> Complete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await prescriptions.CompleteAsync(id, cancellationToken));
        }
        catch (Exception ex)
        {
            return ApiError(ex);
        }
    }
}
