using HospitalLegacy.Api.Data;
using HospitalLegacy.Api.Models;
using HospitalLegacy.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("legacy-frontend", policy =>
    {
        policy.WithOrigins("http://localhost:3001", "http://127.0.0.1:3001")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<HospitalDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Hospital") ?? "Data Source=hospital-legacy.db");
});
builder.Services.AddScoped<HospitalService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("legacy-frontend");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();
    db.Database.EnsureCreated();

    if (!db.Beds.Any())
    {
        var beds = new[]
        {
            new BedRecord { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), Code = "A-101", Ward = "Clinica Medica", Status = "OCUPADO" },
            new BedRecord { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), Code = "A-102", Ward = "Clinica Medica", Status = "DISPONIVEL" },
            new BedRecord { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), Code = "B-201", Ward = "Cirurgia", Status = "DISPONIVEL" },
            new BedRecord { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), Code = "U-301", Ward = "UTI", Status = "DISPONIVEL" },
            new BedRecord { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"), Code = "U-302", Ward = "UTI", Status = "MANUTENCAO" }
        };

        var patient = new PatientRecord
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
            FullName = "Maria Oliveira",
            Cpf = "12345678901",
            BirthDate = new DateTime(1988, 3, 12),
            InsuranceName = "Plano Vida"
        };

        var admission = new AdmissionRecord
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
            PatientId = patient.Id,
            BedId = beds[0].Id,
            Status = "INTERNADO",
            AdmittedAt = DateTime.UtcNow.AddHours(-10)
        };

        db.Beds.AddRange(beds);
        db.Patients.Add(patient);
        db.Admissions.Add(admission);
        db.Prescriptions.Add(new PrescriptionRecord
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-ddddddddddd1"),
            AdmissionId = admission.Id,
            MedicineName = "Dipirona",
            Dose = "500mg",
            FrequencyHours = 6,
            StartAt = DateTime.UtcNow.AddHours(-8),
            EndAt = DateTime.UtcNow.AddHours(16),
            Status = "ATIVA"
        });
        db.ExamRequests.Add(new ExamRequestRecord
        {
            Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeee1"),
            AdmissionId = admission.Id,
            ExamType = "Hemograma",
            Status = "PENDENTE",
            RequestedAt = DateTime.UtcNow.AddHours(-5)
        });
        db.SaveChanges();
    }
}

app.Run();

public partial class Program
{
}
