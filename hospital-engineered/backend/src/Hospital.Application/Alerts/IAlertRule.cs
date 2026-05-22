using Hospital.Application.DTOs;
using Hospital.Domain.Entities;

namespace Hospital.Application.Alerts;

public record HospitalAlertContext(
    IReadOnlyList<Bed> Beds,
    IReadOnlyList<Admission> Admissions,
    IReadOnlyList<Prescription> Prescriptions,
    IReadOnlyList<ExamRequest> Exams);

public interface IAlertRule
{
    IEnumerable<AlertDto> Evaluate(HospitalAlertContext context);
}
