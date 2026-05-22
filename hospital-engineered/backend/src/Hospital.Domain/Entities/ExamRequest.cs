using Hospital.Domain.Common;
using Hospital.Domain.Enums;

namespace Hospital.Domain.Entities;

public class ExamRequest
{
    private ExamRequest()
    {
        ExamType = "";
    }

    internal ExamRequest(Guid id, Guid admissionId, string examType, DateTime requestedAt)
    {
        if (id == Guid.Empty || admissionId == Guid.Empty)
        {
            throw new DomainException("Exame exige identificadores.");
        }

        if (string.IsNullOrWhiteSpace(examType))
        {
            throw new DomainException("Tipo de exame e obrigatorio.");
        }

        Id = id;
        AdmissionId = admissionId;
        ExamType = examType.Trim();
        Status = ExamStatus.Pending;
        RequestedAt = requestedAt;
    }

    public Guid Id { get; private set; }
    public Guid AdmissionId { get; private set; }
    public string ExamType { get; private set; }
    public ExamStatus Status { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public void Complete(DateTime completedAt)
    {
        Status = ExamStatus.Completed;
        CompletedAt = completedAt;
    }
}
