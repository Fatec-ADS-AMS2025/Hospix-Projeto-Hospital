using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Persistence;

public static class HospitalSeedData
{
    public static async Task SeedAsync(HospitalDbContext db, CancellationToken cancellationToken = default)
    {
        await db.Database.EnsureCreatedAsync(cancellationToken);

        if (await db.Beds.AnyAsync(cancellationToken))
        {
            return;
        }

        var beds = new[]
        {
            new Bed(Guid.Parse("11111111-1111-1111-1111-111111111101"), "A-101", "Clinica Medica"),
            new Bed(Guid.Parse("11111111-1111-1111-1111-111111111102"), "A-102", "Clinica Medica"),
            new Bed(Guid.Parse("11111111-1111-1111-1111-111111111103"), "B-201", "Cirurgia"),
            new Bed(Guid.Parse("11111111-1111-1111-1111-111111111104"), "U-301", "UTI"),
            new Bed(Guid.Parse("11111111-1111-1111-1111-111111111105"), "U-302", "UTI", BedStatus.Maintenance)
        };

        var patient = new Patient(
            Guid.Parse("22222222-2222-2222-2222-222222222201"),
            "Maria Oliveira",
            new Cpf("12345678901"),
            new DateTime(1988, 3, 12),
            "Plano Vida");

        var now = DateTime.UtcNow;
        var admission = new Admission(
            Guid.Parse("33333333-3333-3333-3333-333333333301"),
            patient,
            beds[0],
            now.AddHours(-10));

        admission.AddPrescription(
            Guid.Parse("44444444-4444-4444-4444-444444444401"),
            "Dipirona",
            new Dosage("500mg"),
            6,
            new DateRange(now.AddHours(-8), now.AddHours(16)));
        admission.RequestExam(
            Guid.Parse("55555555-5555-5555-5555-555555555501"),
            "Hemograma",
            now.AddHours(-5));

        db.Patients.Add(patient);
        db.Beds.AddRange(beds);
        db.Admissions.Add(admission);
        await db.SaveChangesAsync(cancellationToken);
    }
}
