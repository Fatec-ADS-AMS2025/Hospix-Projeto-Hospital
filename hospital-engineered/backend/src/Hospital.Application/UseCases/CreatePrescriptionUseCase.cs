using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Application.Mapping;
using Hospital.Domain.Common;
using Hospital.Domain.ValueObjects;

namespace Hospital.Application.UseCases;

public class CreatePrescriptionUseCase
{
    private readonly IAdmissionRepository admissions;
    private readonly IUnitOfWork unitOfWork;

    public CreatePrescriptionUseCase(IAdmissionRepository admissions, IUnitOfWork unitOfWork)
    {
        this.admissions = admissions;
        this.unitOfWork = unitOfWork;
    }

    public async Task<PrescriptionDto> ExecuteAsync(CreatePrescriptionRequest request, CancellationToken cancellationToken)
    {
        var admission = await admissions.GetByIdAsync(request.AdmissionId, cancellationToken)
            ?? throw new DomainException("Internacao nao encontrada.");

        var prescription = admission.AddPrescription(
            Guid.NewGuid(),
            request.MedicineName,
            new Dosage(request.Dose),
            request.FrequencyHours,
            new DateRange(request.StartAt, request.EndAt));

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return HospitalMapping.ToDto(prescription);
    }
}
