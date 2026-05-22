using Hospital.Application.DTOs;
using Hospital.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertsController : ControllerBase
{
    private readonly GenerateAlertsUseCase alerts;

    public AlertsController(GenerateAlertsUseCase alerts)
    {
        this.alerts = alerts;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AlertDto>>> List(CancellationToken cancellationToken)
    {
        return Ok(await alerts.ExecuteAsync(cancellationToken));
    }
}
