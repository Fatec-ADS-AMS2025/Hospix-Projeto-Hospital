using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.ValueObjects;
using Hospital.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories;

public class EfHospitalRepository :
    IPatientRepository,
    IBedRepository,
    IAdmissionRepository,
    IPrescriptionRepository,
    IExamRepository,
    IUnitOfWork
{
    private readonly HospitalDbContext db;

    public EfHospitalRepository(HospitalDbContext db)
    {
        this.db = db;
    }

    public async Task<IReadOnlyList<Patient>> ListAsync(CancellationToken cancellationToken)
    {
        return await db.Patients.OrderBy(x => x.FullName).ToListAsync(cancellationToken);
    }

    public Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return db.Patients.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Patient?> FindByCpfAsync(Cpf cpf, CancellationToken cancellationToken)
    {
        return db.Patients.FirstOrDefaultAsync(x => x.Cpf == cpf, cancellationToken);
    }

    public async Task AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        await db.Patients.AddAsync(patient, cancellationToken);
    }

    async Task<IReadOnlyList<Bed>> IBedRepository.ListAsync(CancellationToken cancellationToken)
    {
        return await db.Beds.OrderBy(x => x.Code).ToListAsync(cancellationToken);
    }

    Task<Bed?> IBedRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return db.Beds.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    async Task<IReadOnlyList<Admission>> IAdmissionRepository.ListAsync(CancellationToken cancellationToken)
    {
        return await AdmissionsWithDetails().ToListAsync(cancellationToken);
    }

    Task<Admission?> IAdmissionRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return AdmissionsWithDetails().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    Task<Admission?> IAdmissionRepository.GetByPrescriptionIdAsync(Guid prescriptionId, CancellationToken cancellationToken)
    {
        return AdmissionsWithDetails().FirstOrDefaultAsync(x => x.Prescriptions.Any(p => p.Id == prescriptionId), cancellationToken);
    }

    Task<Admission?> IAdmissionRepository.GetByExamIdAsync(Guid examId, CancellationToken cancellationToken)
    {
        return AdmissionsWithDetails().FirstOrDefaultAsync(x => x.Exams.Any(e => e.Id == examId), cancellationToken);
    }

    Task<bool> IAdmissionRepository.HasActiveAdmissionAsync(Guid patientId, CancellationToken cancellationToken)
    {
        return db.Admissions.AnyAsync(x => x.PatientId == patientId && x.Status == AdmissionStatus.Active, cancellationToken);
    }

    async Task IAdmissionRepository.AddAsync(Admission admission, CancellationToken cancellationToken)
    {
        await db.Admissions.AddAsync(admission, cancellationToken);
    }

    async Task<IReadOnlyList<Prescription>> IPrescriptionRepository.ListAsync(CancellationToken cancellationToken)
    {
        return await db.Prescriptions.OrderByDescending(x => x.StartAt).ToListAsync(cancellationToken);
    }

    async Task<IReadOnlyList<ExamRequest>> IExamRepository.ListAsync(CancellationToken cancellationToken)
    {
        return await db.ExamRequests.OrderByDescending(x => x.RequestedAt).ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return db.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Admission> AdmissionsWithDetails()
    {
        return db.Admissions
            .Include(x => x.Patient)
            .Include(x => x.Bed)
            .Include(x => x.Prescriptions)
            .Include(x => x.Exams)
            .OrderByDescending(x => x.AdmittedAt);
    }
}
