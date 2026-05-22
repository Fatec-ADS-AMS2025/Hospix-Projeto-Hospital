using Hospital.Domain.Enums;
using Hospital.Domain.ValueObjects;

namespace Hospital.Domain.Entities;

public class Invoice
{
    public Invoice(Guid id, Guid admissionId, Money amount, InvoiceStatus status, DateTime createdAt)
    {
        Id = id;
        AdmissionId = admissionId;
        Amount = amount;
        Status = status;
        CreatedAt = createdAt;
    }

    public Guid Id { get; }
    public Guid AdmissionId { get; }
    public Money Amount { get; }
    public InvoiceStatus Status { get; }
    public DateTime CreatedAt { get; }
}
