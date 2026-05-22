namespace HospitalLegacy.Api.Models;

public class PatientRecord
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
    public string Cpf { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public string InsuranceName { get; set; } = "";
}

public class BedRecord
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Ward { get; set; } = "";
    public string Status { get; set; } = "";
}

public class AdmissionRecord
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public PatientRecord? Patient { get; set; }
    public Guid BedId { get; set; }
    public BedRecord? Bed { get; set; }
    public string Status { get; set; } = "";
    public DateTime AdmittedAt { get; set; }
    public DateTime? DischargedAt { get; set; }
    public List<PrescriptionRecord> Prescriptions { get; set; } = [];
    public List<ExamRequestRecord> Exams { get; set; } = [];
}

public class PrescriptionRecord
{
    public Guid Id { get; set; }
    public Guid AdmissionId { get; set; }
    public AdmissionRecord? Admission { get; set; }
    public string MedicineName { get; set; } = "";
    public string Dose { get; set; } = "";
    public int FrequencyHours { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string Status { get; set; } = "";
}

public class ExamRequestRecord
{
    public Guid Id { get; set; }
    public Guid AdmissionId { get; set; }
    public AdmissionRecord? Admission { get; set; }
    public string ExamType { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime RequestedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class InvoiceRecord
{
    public Guid Id { get; set; }
    public Guid AdmissionId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
