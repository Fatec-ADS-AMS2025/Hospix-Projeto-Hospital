using Hospital.Application.DTOs;

namespace Hospital.Application.Interfaces;

public interface IInvoiceGateway
{
    Task NotifyAdmissionDischargedAsync(AdmissionDischargedEvent dischargedEvent, CancellationToken cancellationToken);
    Task<IReadOnlyList<InvoiceDto>> ListInvoicesAsync(CancellationToken cancellationToken);
}
