using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Application.Mapping;
using Hospital.Domain.Common;

namespace Hospital.Application.UseCases;

public class PrescriptionUseCases
{
    private readonly IPrescriptionRepository prescriptions;
    private readonly IAdmissionRepository admissions;
    private readonly IUnitOfWork unitOfWork;
    private readonly IClock clock;

    public PrescriptionUseCases(IPrescriptionRepository prescriptions, IAdmissionRepository admissions, IUnitOfWork unitOfWork, IClock clock)
    {
        this.prescriptions = prescriptions;
        this.admissions = admissions;
        this.unitOfWork = unitOfWork;
        this.clock = clock;
    }

    public async Task<IReadOnlyList<PrescriptionDto>> ListAsync(CancellationToken cancellationToken)
    {
        var result = await prescriptions.ListAsync(cancellationToken);
        return result.OrderByDescending(x => x.StartAt).Select(HospitalMapping.ToDto).ToList();
    }

    public async Task<PrescriptionDto> CompleteAsync(Guid prescriptionId, CancellationToken cancellationToken)
    {
        var admission = await admissions.GetByPrescriptionIdAsync(prescriptionId, cancellationToken)
            ?? throw new DomainException("Prescricao nao encontrada.");

        admission.CompletePrescription(prescriptionId, clock.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return HospitalMapping.ToDto(admission.Prescriptions.First(x => x.Id == prescriptionId));
    }
}
