using System.Net;
using System.Net.Http.Json;
using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Api.Tests;

public class HospitalApiFlowTests
{
    [Fact]
    public async Task Post_patient_creates_patient()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/patients", new CreatePatientRequest(
            "Paciente API " + Guid.NewGuid().ToString("N")[..6],
            Random.Shared.NextInt64(10000000000, 99999999999).ToString(),
            new DateTime(1992, 2, 2),
            "Plano API"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var patient = await response.Content.ReadFromJsonAsync<PatientDto>();
        Assert.NotNull(patient);
    }

    [Fact]
    public async Task Post_admission_admits_patient()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var patient = await CreatePatient(client);
        var bed = (await client.GetFromJsonAsync<List<BedDto>>("/api/beds") ?? []).First(x => x.Status == "DISPONIVEL");

        var admission = await Post<CreateAdmissionRequest, AdmissionDto>(client, "/api/admissions", new CreateAdmissionRequest(patient.Id, bed.Id));

        Assert.Equal("INTERNADO", admission.Status);
        Assert.Equal(patient.Id, admission.PatientId);
    }

    [Fact]
    public async Task Complete_hospital_flow_generates_invoice()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var patient = await CreatePatient(client);
        var bed = (await client.GetFromJsonAsync<List<BedDto>>("/api/beds") ?? []).First(x => x.Status == "DISPONIVEL");
        var admission = await Post<CreateAdmissionRequest, AdmissionDto>(client, "/api/admissions", new CreateAdmissionRequest(patient.Id, bed.Id));
        var prescription = await Post<CreatePrescriptionRequest, PrescriptionDto>(client, "/api/prescriptions", new CreatePrescriptionRequest(
            admission.Id,
            "Amoxicilina",
            "875mg",
            12,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(24)));
        var exam = await Post<CreateExamRequest, ExamRequestDto>(client, "/api/exams", new CreateExamRequest(admission.Id, "Raio-X"));

        await client.PatchAsync($"/api/prescriptions/{prescription.Id}/complete", null);
        await client.PatchAsync($"/api/exams/{exam.Id}/complete", null);
        var discharge = await client.PatchAsync($"/api/admissions/{admission.Id}/discharge", null);

        Assert.Equal(HttpStatusCode.OK, discharge.StatusCode);
        var invoices = await client.GetFromJsonAsync<List<InvoiceDto>>("/api/invoices") ?? [];
        Assert.Contains(invoices, x => x.AdmissionId == admission.Id);
    }

    private static WebApplicationFactory<Program> CreateFactory()
    {
        var dbPath = Path.Combine(Path.GetTempPath(), $"hospital-api-test-{Guid.NewGuid():N}.db");
        var gateway = new FakeInvoiceGateway();

        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                foreach (var descriptor in services.Where(x => x.ServiceType == typeof(DbContextOptions<HospitalDbContext>)).ToList())
                {
                    services.Remove(descriptor);
                }

                foreach (var descriptor in services.Where(x => x.ServiceType == typeof(IInvoiceGateway)).ToList())
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<HospitalDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));
                services.AddSingleton<IInvoiceGateway>(gateway);
            });
        });
    }

    private static async Task<PatientDto> CreatePatient(HttpClient client)
    {
        return await Post<CreatePatientRequest, PatientDto>(client, "/api/patients", new CreatePatientRequest(
            "Paciente Fluxo " + Guid.NewGuid().ToString("N")[..6],
            Random.Shared.NextInt64(10000000000, 99999999999).ToString(),
            new DateTime(1993, 3, 3),
            "Plano Fluxo"));
    }

    private static async Task<TResponse> Post<TRequest, TResponse>(HttpClient client, string route, TRequest request)
    {
        var response = await client.PostAsJsonAsync(route, request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    private class FakeInvoiceGateway : IInvoiceGateway
    {
        private readonly List<InvoiceDto> invoices = [];

        public Task NotifyAdmissionDischargedAsync(AdmissionDischargedEvent dischargedEvent, CancellationToken cancellationToken)
        {
            invoices.Add(new InvoiceDto(Guid.NewGuid(), dischargedEvent.AdmissionId, 1250m, "GERADA", DateTime.UtcNow));
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<InvoiceDto>> ListInvoicesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<InvoiceDto>>(invoices);
        }
    }
}
