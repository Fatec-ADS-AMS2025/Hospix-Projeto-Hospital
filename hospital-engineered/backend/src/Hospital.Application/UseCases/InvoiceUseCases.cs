using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;

namespace Hospital.Application.UseCases;

public class InvoiceUseCases
{
    private readonly IInvoiceGateway invoices;

    public InvoiceUseCases(IInvoiceGateway invoices)
    {
        this.invoices = invoices;
    }

    public Task<IReadOnlyList<InvoiceDto>> ListAsync(CancellationToken cancellationToken)
    {
        return invoices.ListInvoicesAsync(cancellationToken);
    }
}
