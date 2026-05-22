using Hospital.Application.DTOs;
using Hospital.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers;

[ApiController]
[Route("api/exams")]
public class ExamsController : HospitalControllerBase
{
    private readonly ExamUseCases exams;

    public ExamsController(ExamUseCases exams)
    {
        this.exams = exams;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ExamRequestDto>>> List(CancellationToken cancellationToken)
    {
        return Ok(await exams.ListAsync(cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<ExamRequestDto>> Create(CreateExamRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await exams.CreateAsync(request, cancellationToken));
        }
        catch (Exception ex)
        {
            return ApiError(ex);
        }
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<ActionResult<ExamRequestDto>> Complete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await exams.CompleteAsync(id, cancellationToken));
        }
        catch (Exception ex)
        {
            return ApiError(ex);
        }
    }
}
