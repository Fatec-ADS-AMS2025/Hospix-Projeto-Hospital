using Hospital.Domain.Common;
using Hospital.Domain.Enums;
using Hospital.Domain.ValueObjects;

namespace Hospital.Domain.Entities;

public class Prescription
{
    private Prescription()
    {
        MedicineName = "";
        Dose = new Dosage("0mg");
    }

    internal Prescription(Guid id, Guid admissionId, string medicineName, Dosage dose, int frequencyHours, DateRange period)
    {
        if (id == Guid.Empty || admissionId == Guid.Empty)
        {
            throw new DomainException("Prescricao exige identificadores.");
        }

        if (string.IsNullOrWhiteSpace(medicineName))
        {
            throw new DomainException("Medicamento e obrigatorio.");
        }

        if (frequencyHours <= 0)
        {
            throw new DomainException("Frequencia deve ser maior que zero.");
        }

        Id = id;
        AdmissionId = admissionId;
        MedicineName = medicineName.Trim();
        Dose = dose;
        FrequencyHours = frequencyHours;
        StartAt = period.StartAt;
        EndAt = period.EndAt;
        Status = PrescriptionStatus.Active;
    }

    public Guid Id { get; private set; }
    public Guid AdmissionId { get; private set; }
    public string MedicineName { get; private set; }
    public Dosage Dose { get; private set; }
    public int FrequencyHours { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public PrescriptionStatus Status { get; private set; }
    public DateRange Period => new(StartAt, EndAt);

    public void Complete(DateTime completedAt)
    {
        Status = PrescriptionStatus.Completed;
        EndAt = completedAt;
    }
}
