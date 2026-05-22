using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Application.Mapping;
using Hospital.Domain.Common;
using Hospital.Domain.Entities;
using Hospital.Domain.ValueObjects;

namespace Hospital.Application.UseCases;

public class PatientUseCases
{
    private readonly IPatientRepository patients;
    private readonly IUnitOfWork unitOfWork;

    public PatientUseCases(IPatientRepository patients, IUnitOfWork unitOfWork)
    {
        this.patients = patients;
        this.unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<PatientDto>> ListAsync(CancellationToken cancellationToken)
    {
        var result = await patients.ListAsync(cancellationToken);
        return result.Select(HospitalMapping.ToDto).ToList();
    }

    public async Task<PatientDto> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken)
    {
        var cpf = new Cpf(request.Cpf);
        if (await patients.FindByCpfAsync(cpf, cancellationToken) is not null)
        {
            throw new DomainException("CPF ja cadastrado.");
        }

        var patient = new Patient(Guid.NewGuid(), request.FullName, cpf, request.BirthDate, request.InsuranceName);
        await patients.AddAsync(patient, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return HospitalMapping.ToDto(patient);
    }
}
