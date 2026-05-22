using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Application.Mapping;
using Hospital.Domain.Enums;

namespace Hospital.Application.UseCases;

public class DashboardUseCase
{
    private readonly IPatientRepository patients;
    private readonly IBedRepository beds;
    private readonly IAdmissionRepository admissions;
    private readonly IPrescriptionRepository prescriptions;
    private readonly IExamRepository exams;
    private readonly IInvoiceGateway invoices;
    private readonly GenerateAlertsUseCase alerts;

    public DashboardUseCase(
        IPatientRepository patients,
        IBedRepository beds,
        IAdmissionRepository admissions,
        IPrescriptionRepository prescriptions,
        IExamRepository exams,
        IInvoiceGateway invoices,
        GenerateAlertsUseCase alerts)
    {
        this.patients = patients;
        this.beds = beds;
        this.admissions = admissions;
        this.prescriptions = prescriptions;
        this.exams = exams;
        this.invoices = invoices;
        this.alerts = alerts;
    }

    public async Task<DashboardDto> ExecuteAsync(CancellationToken cancellationToken)
    {
        var patientList = await patients.ListAsync(cancellationToken);
        var bedList = await beds.ListAsync(cancellationToken);
        var admissionList = await admissions.ListAsync(cancellationToken);
        var prescriptionList = await prescriptions.ListAsync(cancellationToken);
        var examList = await exams.ListAsync(cancellationToken);
        var invoiceList = await invoices.ListInvoicesAsync(cancellationToken);

        return new DashboardDto(
            patientList.Count,
            admissionList.Count(x => x.Status == AdmissionStatus.Active),
            bedList.Count(x => x.Status == BedStatus.Occupied),
            examList.Count(x => x.Status == ExamStatus.Pending),
            prescriptionList.Count(x => x.Status == PrescriptionStatus.Active),
            invoiceList.Count,
            admissionList.OrderByDescending(x => x.AdmittedAt).Select(HospitalMapping.ToDto).ToList(),
            await alerts.ExecuteAsync(cancellationToken));
    }
}
