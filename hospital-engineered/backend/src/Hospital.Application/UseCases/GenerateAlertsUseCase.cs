using Hospital.Application.Alerts;
using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;

namespace Hospital.Application.UseCases;

public class GenerateAlertsUseCase
{
    private readonly IEnumerable<IAlertRule> rules;
    private readonly IBedRepository beds;
    private readonly IAdmissionRepository admissions;
    private readonly IPrescriptionRepository prescriptions;
    private readonly IExamRepository exams;

    public GenerateAlertsUseCase(
        IEnumerable<IAlertRule> rules,
        IBedRepository beds,
        IAdmissionRepository admissions,
        IPrescriptionRepository prescriptions,
        IExamRepository exams)
    {
        this.rules = rules;
        this.beds = beds;
        this.admissions = admissions;
        this.prescriptions = prescriptions;
        this.exams = exams;
    }

    public async Task<IReadOnlyList<AlertDto>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var context = new HospitalAlertContext(
            await beds.ListAsync(cancellationToken),
            await admissions.ListAsync(cancellationToken),
            await prescriptions.ListAsync(cancellationToken),
            await exams.ListAsync(cancellationToken));

        return rules.SelectMany(rule => rule.Evaluate(context)).ToList();
    }
}
