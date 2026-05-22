using Hospital.Domain.Common;
using Hospital.Domain.Enums;
using Hospital.Domain.ValueObjects;

namespace Hospital.Domain.Entities;

public class Admission
{
    private Admission()
    {
        Patient = null!;
        Bed = null!;
    }

    public Admission(Guid id, Patient patient, Bed bed, DateTime admittedAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("Internacao exige identificador.");
        }

        Id = id;
        PatientId = patient.Id;
        Patient = patient;
        BedId = bed.Id;
        Bed = bed;
        Status = AdmissionStatus.Active;
        AdmittedAt = admittedAt;

        bed.Occupy();
    }

    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public Patient Patient { get; private set; }
    public Guid BedId { get; private set; }
    public Bed Bed { get; private set; }
    public AdmissionStatus Status { get; private set; }
    public DateTime AdmittedAt { get; private set; }
    public DateTime? DischargedAt { get; private set; }
    public List<Prescription> Prescriptions { get; private set; } = [];
    public List<ExamRequest> Exams { get; private set; } = [];

    public Prescription AddPrescription(Guid prescriptionId, string medicineName, Dosage dose, int frequencyHours, DateRange period)
    {
        EnsureActive("Prescricao exige internacao ativa.");
        var prescription = new Prescription(prescriptionId, Id, medicineName, dose, frequencyHours, period);
        Prescriptions.Add(prescription);
        return prescription;
    }

    public ExamRequest RequestExam(Guid examId, string examType, DateTime requestedAt)
    {
        EnsureActive("Exame exige internacao ativa.");
        var exam = new ExamRequest(examId, Id, examType, requestedAt);
        Exams.Add(exam);
        return exam;
    }

    public void CompletePrescription(Guid prescriptionId, DateTime completedAt)
    {
        var prescription = Prescriptions.FirstOrDefault(x => x.Id == prescriptionId);
        if (prescription is null)
        {
            throw new DomainException("Prescricao nao encontrada nesta internacao.");
        }

        prescription.Complete(completedAt);
    }

    public void CompleteExam(Guid examId, DateTime completedAt)
    {
        var exam = Exams.FirstOrDefault(x => x.Id == examId);
        if (exam is null)
        {
            throw new DomainException("Exame nao encontrado nesta internacao.");
        }

        exam.Complete(completedAt);
    }

    public bool HasPendingExams => Exams.Any(x => x.Status == ExamStatus.Pending);
    public bool HasActivePrescriptions => Prescriptions.Any(x => x.Status == PrescriptionStatus.Active);
    public bool IsDischargeBlocked => Status == AdmissionStatus.Active && (HasPendingExams || HasActivePrescriptions);

    public void Discharge(DateTime dischargedAt)
    {
        EnsureActive("Internacao ja encerrada.");

        if (HasPendingExams)
        {
            throw new DomainException("Alta bloqueada por exame pendente.");
        }

        if (HasActivePrescriptions)
        {
            throw new DomainException("Alta bloqueada por prescricao ativa.");
        }

        Status = AdmissionStatus.Discharged;
        DischargedAt = dischargedAt;
        Bed.Release();
    }

    private void EnsureActive(string message)
    {
        if (Status != AdmissionStatus.Active)
        {
            throw new DomainException(message);
        }
    }
}
