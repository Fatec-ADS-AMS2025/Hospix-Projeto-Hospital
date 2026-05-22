using Hospital.Application.DTOs;
using Hospital.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers;

[ApiController]
[Route("api/admissions")]
public class AdmissionsController : HospitalControllerBase
{
    private readonly AdmissionUseCases admissions;

    public AdmissionsController(AdmissionUseCases admissions)
    {
        this.admissions = admissions;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdmissionDto>>> List(CancellationToken cancellationToken)
    {
        return Ok(await admissions.ListAsync(cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<AdmissionDto>> Create(CreateAdmissionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await admissions.CreateAsync(request, cancellationToken));
        }
        catch (Exception ex)
        {
            return ApiError(ex);
        }
    }

    [HttpPatch("{id:guid}/discharge")]
    public async Task<ActionResult<AdmissionDto>> Discharge(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await admissions.DischargeAsync(id, cancellationToken));
        }
        catch (Exception ex)
        {
            return ApiError(ex);
        }
    }
}
