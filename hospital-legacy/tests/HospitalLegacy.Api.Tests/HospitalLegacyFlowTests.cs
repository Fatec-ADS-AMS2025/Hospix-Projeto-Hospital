using System.Net;
using System.Net.Http.Json;
using HospitalLegacy.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HospitalLegacy.Api.Tests;

public class HospitalLegacyFlowTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public HospitalLegacyFlowTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task Dashboard_smoke_test_returns_success()
    {
        var response = await client.GetAsync("/api/dashboard");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Complete_happy_path_creates_invoice()
    {
        var patient = await Post<CreatePatientRequest, PatientDto>("/api/patients", new CreatePatientRequest(
            "Teste Legacy " + Guid.NewGuid().ToString("N")[..8],
            Random.Shared.NextInt64(10000000000, 99999999999).ToString(),
            new DateTime(1991, 8, 20),
            "Plano Teste"));

        var beds = await client.GetFromJsonAsync<List<BedDto>>("/api/beds") ?? [];
        var freeBed = beds.First(x => x.Status == "DISPONIVEL");

        var admission = await Post<CreateAdmissionRequest, AdmissionDto>("/api/admissions", new CreateAdmissionRequest(patient.Id, freeBed.Id));
        var prescription = await Post<CreatePrescriptionRequest, PrescriptionDto>("/api/prescriptions", new CreatePrescriptionRequest(
            admission.Id,
            "Amoxicilina",
            "875mg",
            12,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(24)));
        var exam = await Post<CreateExamRequest, ExamRequestDto>("/api/exams", new CreateExamRequest(admission.Id, "Raio-X"));

        await client.PatchAsync($"/api/prescriptions/{prescription.Id}/complete", null);
        await client.PatchAsync($"/api/exams/{exam.Id}/complete", null);
        var discharge = await client.PatchAsync($"/api/admissions/{admission.Id}/discharge", null);

        Assert.Equal(HttpStatusCode.OK, discharge.StatusCode);
        var invoices = await client.GetFromJsonAsync<List<InvoiceDto>>("/api/invoices") ?? [];
        Assert.Contains(invoices, x => x.AdmissionId == admission.Id);
    }

    private async Task<TResponse> Post<TRequest, TResponse>(string route, TRequest request)
    {
        var response = await client.PostAsJsonAsync(route, request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }
}
