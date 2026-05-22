using Hospital.Application.DTOs;
using Hospital.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly InvoiceUseCases invoices;

    public InvoicesController(InvoiceUseCases invoices)
    {
        this.invoices = invoices;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<InvoiceDto>>> List(CancellationToken cancellationToken)
    {
        return Ok(await invoices.ListAsync(cancellationToken));
    }
}
