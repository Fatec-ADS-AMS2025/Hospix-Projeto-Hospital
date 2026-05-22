using Hospital.Application.DTOs;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;

namespace Hospital.Application.Mapping;

public static class HospitalMapping
{
    public static PatientDto ToDto(Patient patient)
    {
        return new PatientDto(patient.Id, patient.FullName, patient.Cpf.Value, patient.BirthDate, patient.InsuranceName);
    }

    public static BedDto ToDto(Bed bed)
    {
        return new BedDto(bed.Id, bed.Code, bed.Ward, ToStatusText(bed.Status));
    }

    public static AdmissionDto ToDto(Admission admission)
    {
        return new AdmissionDto(
            admission.Id,
            admission.PatientId,
            admission.Patient.FullName,
            admission.Bed.Code,
            ToStatusText(admission.Status),
            admission.AdmittedAt,
            admission.DischargedAt);
    }

    public static PrescriptionDto ToDto(Prescription prescription)
    {
        return new PrescriptionDto(
            prescription.Id,
            prescription.AdmissionId,
            prescription.MedicineName,
            prescription.Dose.Value,
            prescription.FrequencyHours,
            prescription.StartAt,
            prescription.EndAt,
            ToStatusText(prescription.Status));
    }

    public static ExamRequestDto ToDto(ExamRequest exam)
    {
        return new ExamRequestDto(exam.Id, exam.AdmissionId, exam.ExamType, ToStatusText(exam.Status), exam.RequestedAt, exam.CompletedAt);
    }

    public static string ToStatusText(BedStatus status) => status switch
    {
        BedStatus.Available => "DISPONIVEL",
        BedStatus.Occupied => "OCUPADO",
        BedStatus.Maintenance => "MANUTENCAO",
        _ => status.ToString()
    };

    public static string ToStatusText(AdmissionStatus status) => status switch
    {
        AdmissionStatus.Active => "INTERNADO",
        AdmissionStatus.Discharged => "ALTA",
        _ => status.ToString()
    };

    public static string ToStatusText(PrescriptionStatus status) => status switch
    {
        PrescriptionStatus.Active => "ATIVA",
        PrescriptionStatus.Completed => "FINALIZADA",
        _ => status.ToString()
    };

    public static string ToStatusText(ExamStatus status) => status switch
    {
        ExamStatus.Pending => "PENDENTE",
        ExamStatus.Completed => "CONCLUIDO",
        _ => status.ToString()
    };
}
