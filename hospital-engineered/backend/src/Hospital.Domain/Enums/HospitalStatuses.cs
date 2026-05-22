namespace Hospital.Domain.Enums;

public enum BedStatus
{
    Available,
    Occupied,
    Maintenance
}

public enum AdmissionStatus
{
    Active,
    Discharged
}

public enum PrescriptionStatus
{
    Active,
    Completed
}

public enum ExamStatus
{
    Pending,
    Completed
}

public enum InvoiceStatus
{
    Generated,
    Paid,
    Canceled
}
