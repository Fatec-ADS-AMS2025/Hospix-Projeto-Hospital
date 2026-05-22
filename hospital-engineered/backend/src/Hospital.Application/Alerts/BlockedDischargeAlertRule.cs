using Hospital.Application.DTOs;

namespace Hospital.Application.Alerts;

public class BlockedDischargeAlertRule : IAlertRule
{
    public IEnumerable<AlertDto> Evaluate(HospitalAlertContext context)
    {
        foreach (var admission in context.Admissions.Where(x => x.IsDischargeBlocked))
        {
            yield return new AlertDto("warning", "Alta bloqueada", $"{admission.Patient.FullName} possui pendencias antes da alta.", "internacao");
        }
    }
}
