using Hospital.Application.DTOs;
using Hospital.Domain.Enums;

namespace Hospital.Application.Alerts;

public class PendingExamAlertRule : IAlertRule
{
    public IEnumerable<AlertDto> Evaluate(HospitalAlertContext context)
    {
        var pending = context.Exams.Count(x => x.Status == ExamStatus.Pending);
        if (pending > 0)
        {
            yield return new AlertDto("danger", "Exames pendentes", $"{pending} exame(s) aguardando conclusao.", "exames");
        }
    }
}
