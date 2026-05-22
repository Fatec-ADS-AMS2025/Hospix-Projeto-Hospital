using System.Net.Http.Json;
using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;

namespace Hospital.Infrastructure.Services;

public class HttpBillingGateway : IInvoiceGateway
{
    private readonly HttpClient client;

    public HttpBillingGateway(HttpClient client)
    {
        this.client = client;
    }

    public async Task NotifyAdmissionDischargedAsync(AdmissionDischargedEvent dischargedEvent, CancellationToken cancellationToken)
    {
        var response = await client.PostAsJsonAsync("/api/billing/admission-discharged", dischargedEvent, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IReadOnlyList<InvoiceDto>> ListInvoicesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await client.GetFromJsonAsync<List<InvoiceDto>>("/api/invoices", cancellationToken) ?? [];
        }
        catch (HttpRequestException)
        {
            return [];
        }
    }
}
