using Hospital.Application.DTOs;
using Hospital.Domain.Enums;

namespace Hospital.Application.Alerts;

public class ActivePrescriptionAlertRule : IAlertRule
{
    public IEnumerable<AlertDto> Evaluate(HospitalAlertContext context)
    {
        var active = context.Prescriptions.Count(x => x.Status == PrescriptionStatus.Active);
        if (active > 0)
        {
            yield return new AlertDto("info", "Prescricoes ativas", $"{active} prescricao(oes) em administracao.", "prescricoes");
        }
    }
}
