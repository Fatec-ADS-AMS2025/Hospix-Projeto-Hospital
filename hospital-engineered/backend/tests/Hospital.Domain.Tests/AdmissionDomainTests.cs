using Hospital.Domain.Common;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.ValueObjects;

namespace Hospital.Domain.Tests;

public class AdmissionDomainTests
{
    [Fact]
    public void Admission_requires_available_bed()
    {
        var patient = Patient();
        var bed = new Bed(Guid.NewGuid(), "M-101", "UTI", BedStatus.Maintenance);

        Assert.Throws<DomainException>(() => new Admission(Guid.NewGuid(), patient, bed, DateTime.UtcNow));
    }

    [Fact]
    public void Discharge_is_blocked_when_exam_is_pending()
    {
        var admission = ActiveAdmission();
        admission.RequestExam(Guid.NewGuid(), "Hemograma", DateTime.UtcNow);

        var error = Assert.Throws<DomainException>(() => admission.Discharge(DateTime.UtcNow));

        Assert.Contains("exame pendente", error.Message);
    }

    [Fact]
    public void Discharge_is_blocked_when_prescription_is_active()
    {
        var admission = ActiveAdmission();
        admission.AddPrescription(
            Guid.NewGuid(),
            "Dipirona",
            new Dosage("500mg"),
            6,
            new DateRange(DateTime.UtcNow, DateTime.UtcNow.AddHours(12)));

        var error = Assert.Throws<DomainException>(() => admission.Discharge(DateTime.UtcNow));

        Assert.Contains("prescricao ativa", error.Message);
    }

    [Fact]
    public void Discharge_succeeds_after_pending_items_are_completed()
    {
        var admission = ActiveAdmission();
        var prescription = admission.AddPrescription(
            Guid.NewGuid(),
            "Dipirona",
            new Dosage("500mg"),
            6,
            new DateRange(DateTime.UtcNow, DateTime.UtcNow.AddHours(12)));
        var exam = admission.RequestExam(Guid.NewGuid(), "Hemograma", DateTime.UtcNow);

        admission.CompletePrescription(prescription.Id, DateTime.UtcNow);
        admission.CompleteExam(exam.Id, DateTime.UtcNow);
        admission.Discharge(DateTime.UtcNow);

        Assert.Equal(AdmissionStatus.Discharged, admission.Status);
        Assert.Equal(BedStatus.Available, admission.Bed.Status);
    }

    private static Admission ActiveAdmission()
    {
        return new Admission(Guid.NewGuid(), Patient(), new Bed(Guid.NewGuid(), "A-101", "Clinica"), DateTime.UtcNow);
    }

    private static Patient Patient()
    {
        return new Patient(Guid.NewGuid(), "Paciente Teste", new Cpf("12345678901"), new DateTime(1990, 1, 1), "Plano");
    }
}
