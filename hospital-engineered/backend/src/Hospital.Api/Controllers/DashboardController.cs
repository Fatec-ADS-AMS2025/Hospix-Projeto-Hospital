using Hospital.Application.DTOs;
using Hospital.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : HospitalControllerBase
{
    private readonly DashboardUseCase dashboard;

    public DashboardController(DashboardUseCase dashboard)
    {
        this.dashboard = dashboard;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> Get(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await dashboard.ExecuteAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            return ApiError(ex);
        }
    }
}
