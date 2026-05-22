using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Application.Mapping;
using Hospital.Domain.Common;

namespace Hospital.Application.UseCases;

public class ExamUseCases
{
    private readonly IExamRepository exams;
    private readonly IAdmissionRepository admissions;
    private readonly IUnitOfWork unitOfWork;
    private readonly IClock clock;

    public ExamUseCases(IExamRepository exams, IAdmissionRepository admissions, IUnitOfWork unitOfWork, IClock clock)
    {
        this.exams = exams;
        this.admissions = admissions;
        this.unitOfWork = unitOfWork;
        this.clock = clock;
    }

    public async Task<IReadOnlyList<ExamRequestDto>> ListAsync(CancellationToken cancellationToken)
    {
        var result = await exams.ListAsync(cancellationToken);
        return result.OrderByDescending(x => x.RequestedAt).Select(HospitalMapping.ToDto).ToList();
    }

    public async Task<ExamRequestDto> CreateAsync(CreateExamRequest request, CancellationToken cancellationToken)
    {
        var admission = await admissions.GetByIdAsync(request.AdmissionId, cancellationToken)
            ?? throw new DomainException("Internacao nao encontrada.");

        var exam = admission.RequestExam(Guid.NewGuid(), request.ExamType, clock.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return HospitalMapping.ToDto(exam);
    }

    public async Task<ExamRequestDto> CompleteAsync(Guid examId, CancellationToken cancellationToken)
    {
        var admission = await admissions.GetByExamIdAsync(examId, cancellationToken)
            ?? throw new DomainException("Exame nao encontrado.");

        admission.CompleteExam(examId, clock.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return HospitalMapping.ToDto(admission.Exams.First(x => x.Id == examId));
    }
}
