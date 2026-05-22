using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Application.Mapping;
using Hospital.Domain.Common;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;

namespace Hospital.Application.UseCases;

public class AdmissionUseCases
{
    private readonly IAdmissionRepository admissions;
    private readonly IPatientRepository patients;
    private readonly IBedRepository beds;
    private readonly IInvoiceGateway invoices;
    private readonly IUnitOfWork unitOfWork;
    private readonly IClock clock;

    public AdmissionUseCases(
        IAdmissionRepository admissions,
        IPatientRepository patients,
        IBedRepository beds,
        IInvoiceGateway invoices,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        this.admissions = admissions;
        this.patients = patients;
        this.beds = beds;
        this.invoices = invoices;
        this.unitOfWork = unitOfWork;
        this.clock = clock;
    }

    public async Task<IReadOnlyList<AdmissionDto>> ListAsync(CancellationToken cancellationToken)
    {
        var result = await admissions.ListAsync(cancellationToken);
        return result.OrderByDescending(x => x.AdmittedAt).Select(HospitalMapping.ToDto).ToList();
    }

    public async Task<AdmissionDto> CreateAsync(CreateAdmissionRequest request, CancellationToken cancellationToken)
    {
        var patient = await patients.GetByIdAsync(request.PatientId, cancellationToken)
            ?? throw new DomainException("Paciente nao encontrado.");

        if (await admissions.HasActiveAdmissionAsync(patient.Id, cancellationToken))
        {
            throw new DomainException("Paciente ja esta internado.");
        }

        var bed = await beds.GetByIdAsync(request.BedId, cancellationToken)
            ?? throw new DomainException("Leito nao encontrado.");

        var admission = new Admission(Guid.NewGuid(), patient, bed, clock.UtcNow);
        await admissions.AddAsync(admission, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return HospitalMapping.ToDto(admission);
    }

    public async Task<AdmissionDto> DischargeAsync(Guid admissionId, CancellationToken cancellationToken)
    {
        var admission = await admissions.GetByIdAsync(admissionId, cancellationToken)
            ?? throw new DomainException("Internacao nao encontrada.");

        admission.Discharge(clock.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await invoices.NotifyAdmissionDischargedAsync(new AdmissionDischargedEvent(
            admission.Id,
            admission.Patient.FullName,
            admission.AdmittedAt,
            admission.DischargedAt!.Value,
            admission.Exams.Count,
            admission.Prescriptions.Count), cancellationToken);

        return HospitalMapping.ToDto(admission);
    }

    public static int CountActive(IEnumerable<Admission> result)
    {
        return result.Count(x => x.Status == AdmissionStatus.Active);
    }
}
