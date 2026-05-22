using Hospital.Application.DTOs;
using Hospital.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : HospitalControllerBase
{
    private readonly PatientUseCases patients;

    public PatientsController(PatientUseCases patients)
    {
        this.patients = patients;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatientDto>>> List(CancellationToken cancellationToken)
    {
        return Ok(await patients.ListAsync(cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create(CreatePatientRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await patients.CreateAsync(request, cancellationToken));
        }
        catch (Exception ex)
        {
            return ApiError(ex);
        }
    }
}
