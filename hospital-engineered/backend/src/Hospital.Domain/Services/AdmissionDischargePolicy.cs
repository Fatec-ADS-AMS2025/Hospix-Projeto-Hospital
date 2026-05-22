using Hospital.Domain.Entities;

namespace Hospital.Domain.Services;

public class AdmissionDischargePolicy
{
    public bool CanDischarge(Admission admission)
    {
        return !admission.HasPendingExams && !admission.HasActivePrescriptions;
    }

    public string ExplainBlock(Admission admission)
    {
        if (admission.HasPendingExams)
        {
            return "Exames pendentes bloqueiam a alta.";
        }

        if (admission.HasActivePrescriptions)
        {
            return "Prescricoes ativas bloqueiam a alta.";
        }

        return "Alta liberada.";
    }
}
