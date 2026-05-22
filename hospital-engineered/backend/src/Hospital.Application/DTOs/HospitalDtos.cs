namespace Hospital.Application.DTOs;

public record PatientDto(Guid Id, string FullName, string Cpf, DateTime BirthDate, string InsuranceName);
public record BedDto(Guid Id, string Code, string Ward, string Status);
public record AdmissionDto(Guid Id, Guid PatientId, string PatientName, string BedCode, string Status, DateTime AdmittedAt, DateTime? DischargedAt);
public record PrescriptionDto(Guid Id, Guid AdmissionId, string MedicineName, string Dose, int FrequencyHours, DateTime StartAt, DateTime EndAt, string Status);
public record ExamRequestDto(Guid Id, Guid AdmissionId, string ExamType, string Status, DateTime RequestedAt, DateTime? CompletedAt);
public record InvoiceDto(Guid Id, Guid AdmissionId, decimal Amount, string Status, DateTime CreatedAt);
public record AlertDto(string Level, string Title, string Message, string Source);

public record DashboardDto(
    int Patients,
    int ActiveAdmissions,
    int OccupiedBeds,
    int PendingExams,
    int ActivePrescriptions,
    int Invoices,
    IReadOnlyList<AdmissionDto> Admissions,
    IReadOnlyList<AlertDto> Alerts);

public record CreatePatientRequest(string FullName, string Cpf, DateTime BirthDate, string InsuranceName);
public record CreateAdmissionRequest(Guid PatientId, Guid BedId);
public record CreatePrescriptionRequest(Guid AdmissionId, string MedicineName, string Dose, int FrequencyHours, DateTime StartAt, DateTime EndAt);
public record CreateExamRequest(Guid AdmissionId, string ExamType);
public record AdmissionDischargedEvent(Guid AdmissionId, string PatientName, DateTime AdmittedAt, DateTime DischargedAt, int ExamCount, int PrescriptionCount);
