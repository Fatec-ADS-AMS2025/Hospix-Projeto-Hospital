using Hospital.Domain.Entities;
using Hospital.Domain.ValueObjects;

namespace Hospital.Application.Interfaces;

public interface IPatientRepository
{
    Task<IReadOnlyList<Patient>> ListAsync(CancellationToken cancellationToken);
    Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Patient?> FindByCpfAsync(Cpf cpf, CancellationToken cancellationToken);
    Task AddAsync(Patient patient, CancellationToken cancellationToken);
}

public interface IBedRepository
{
    Task<IReadOnlyList<Bed>> ListAsync(CancellationToken cancellationToken);
    Task<Bed?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}

public interface IAdmissionRepository
{
    Task<IReadOnlyList<Admission>> ListAsync(CancellationToken cancellationToken);
    Task<Admission?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Admission?> GetByPrescriptionIdAsync(Guid prescriptionId, CancellationToken cancellationToken);
    Task<Admission?> GetByExamIdAsync(Guid examId, CancellationToken cancellationToken);
    Task<bool> HasActiveAdmissionAsync(Guid patientId, CancellationToken cancellationToken);
    Task AddAsync(Admission admission, CancellationToken cancellationToken);
}

public interface IPrescriptionRepository
{
    Task<IReadOnlyList<Prescription>> ListAsync(CancellationToken cancellationToken);
}

public interface IExamRepository
{
    Task<IReadOnlyList<ExamRequest>> ListAsync(CancellationToken cancellationToken);
}

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
