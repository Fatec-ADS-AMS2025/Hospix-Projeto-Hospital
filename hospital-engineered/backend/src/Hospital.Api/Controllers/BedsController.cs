using Hospital.Application.DTOs;
using Hospital.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers;

[ApiController]
[Route("api/beds")]
public class BedsController : ControllerBase
{
    private readonly BedUseCases beds;

    public BedsController(BedUseCases beds)
    {
        this.beds = beds;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BedDto>>> List(CancellationToken cancellationToken)
    {
        return Ok(await beds.ListAsync(cancellationToken));
    }
}
