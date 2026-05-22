using HospitalLegacy.Api.Data;
using HospitalLegacy.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalLegacy.Api.Services;

public class HospitalService
{
    private readonly HospitalDbContext db;

    public HospitalService(HospitalDbContext db)
    {
        this.db = db;
    }

    public DashboardDto GetDashboard()
    {
        var admissions = db.Admissions.Include(x => x.Patient).Include(x => x.Bed).ToList();
        var allPrescriptions = db.Prescriptions.ToList();
        var allExams = db.ExamRequests.ToList();
        var alerts = GetAlerts();

        return new DashboardDto(
            db.Patients.Count(),
            admissions.Count(x => x.Status == "INTERNADO"),
            db.Beds.Count(x => x.Status == "OCUPADO"),
            allExams.Count(x => x.Status == "PENDENTE"),
            allPrescriptions.Count(x => x.Status == "ATIVA"),
            db.Invoices.Count(),
            admissions.OrderByDescending(x => x.AdmittedAt).Select(ToAdmissionDto).ToList(),
            alerts);
    }

    public List<PatientDto> GetPatients()
    {
        return db.Patients.OrderBy(x => x.FullName).Select(x => new PatientDto(x.Id, x.FullName, x.Cpf, x.BirthDate, x.InsuranceName)).ToList();
    }

    public PatientDto CreatePatient(CreatePatientRequest request)
    {
        if (request.FullName == null || request.FullName.Trim().Length < 3)
        {
            throw new InvalidOperationException("Nome do paciente invalido.");
        }

        if (db.Patients.Any(x => x.Cpf == request.Cpf))
        {
            throw new InvalidOperationException("CPF ja cadastrado.");
        }

        var p = new PatientRecord
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Cpf = request.Cpf.Trim(),
            BirthDate = request.BirthDate,
            InsuranceName = string.IsNullOrWhiteSpace(request.InsuranceName) ? "Particular" : request.InsuranceName.Trim()
        };

        db.Patients.Add(p);
        db.SaveChanges();
        return new PatientDto(p.Id, p.FullName, p.Cpf, p.BirthDate, p.InsuranceName);
    }

    public List<BedDto> GetBeds()
    {
        return db.Beds.OrderBy(x => x.Code).Select(x => new BedDto(x.Id, x.Code, x.Ward, x.Status)).ToList();
    }

    public List<AdmissionDto> GetAdmissions()
    {
        return db.Admissions.Include(x => x.Patient).Include(x => x.Bed).OrderByDescending(x => x.AdmittedAt).Select(ToAdmissionDto).ToList();
    }

    public AdmissionDto CreateAdmission(CreateAdmissionRequest request)
    {
        var patient = db.Patients.FirstOrDefault(x => x.Id == request.PatientId);
        if (patient == null)
        {
            throw new InvalidOperationException("Paciente nao encontrado.");
        }

        var alreadyInside = db.Admissions.Any(x => x.PatientId == request.PatientId && x.Status == "INTERNADO");
        if (alreadyInside)
        {
            throw new InvalidOperationException("Paciente ja esta internado.");
        }

        var bed = db.Beds.FirstOrDefault(x => x.Id == request.BedId);
        if (bed == null)
        {
            throw new InvalidOperationException("Leito nao encontrado.");
        }

        if (bed.Status != "DISPONIVEL")
        {
            throw new InvalidOperationException("Leito indisponivel.");
        }

        var admission = new AdmissionRecord
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            Patient = patient,
            BedId = bed.Id,
            Bed = bed,
            Status = "INTERNADO",
            AdmittedAt = DateTime.UtcNow
        };

        bed.Status = "OCUPADO";
        db.Admissions.Add(admission);
        db.SaveChanges();
        return ToAdmissionDto(admission);
    }

    public AdmissionDto Discharge(Guid admissionId)
    {
        var admission = db.Admissions
            .Include(x => x.Patient)
            .Include(x => x.Bed)
            .Include(x => x.Prescriptions)
            .Include(x => x.Exams)
            .FirstOrDefault(x => x.Id == admissionId);

        if (admission == null)
        {
            throw new InvalidOperationException("Internacao nao encontrada.");
        }

        if (admission.Status != "INTERNADO")
        {
            throw new InvalidOperationException("Internacao nao esta ativa.");
        }

        var pendingExam = admission.Exams.Any(x => x.Status == "PENDENTE");
        var activePrescription = admission.Prescriptions.Any(x => x.Status == "ATIVA");

        if (pendingExam || activePrescription)
        {
            var reason = "";
            if (pendingExam)
            {
                reason += "exame pendente ";
            }
            if (activePrescription)
            {
                reason += "prescricao ativa ";
            }
            throw new InvalidOperationException("Alta bloqueada por " + reason.Trim() + ".");
        }

        admission.Status = "ALTA";
        admission.DischargedAt = DateTime.UtcNow;
        if (admission.Bed != null)
        {
            admission.Bed.Status = "DISPONIVEL";
        }

        var hours = Math.Max(1, (int)Math.Ceiling((admission.DischargedAt.Value - admission.AdmittedAt).TotalHours));
        var amount = 350m + (hours * 45m) + (admission.Exams.Count * 180m) + (admission.Prescriptions.Count * 90m);
        db.Invoices.Add(new InvoiceRecord
        {
            Id = Guid.NewGuid(),
            AdmissionId = admission.Id,
            Amount = amount,
            Status = "GERADA",
            CreatedAt = DateTime.UtcNow
        });

        db.SaveChanges();
        return ToAdmissionDto(admission);
    }

    public List<PrescriptionDto> GetPrescriptions()
    {
        return db.Prescriptions.OrderByDescending(x => x.StartAt).Select(ToPrescriptionDto).ToList();
    }

    public PrescriptionDto CreatePrescription(CreatePrescriptionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.MedicineName) || string.IsNullOrWhiteSpace(request.Dose))
        {
            throw new InvalidOperationException("Medicamento e dose sao obrigatorios.");
        }

        if (request.FrequencyHours <= 0)
        {
            throw new InvalidOperationException("Frequencia invalida.");
        }

        if (request.EndAt <= request.StartAt)
        {
            throw new InvalidOperationException("Fim da prescricao deve ser posterior ao inicio.");
        }

        var admission = db.Admissions.FirstOrDefault(x => x.Id == request.AdmissionId);
        if (admission == null || admission.Status != "INTERNADO")
        {
            throw new InvalidOperationException("Prescricao exige internacao ativa.");
        }

        var prescription = new PrescriptionRecord
        {
            Id = Guid.NewGuid(),
            AdmissionId = request.AdmissionId,
            MedicineName = request.MedicineName.Trim(),
            Dose = request.Dose.Trim(),
            FrequencyHours = request.FrequencyHours,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            Status = "ATIVA"
        };

        db.Prescriptions.Add(prescription);
        db.SaveChanges();
        return ToPrescriptionDto(prescription);
    }

    public PrescriptionDto CompletePrescription(Guid prescriptionId)
    {
        var p = db.Prescriptions.FirstOrDefault(x => x.Id == prescriptionId);
        if (p == null)
        {
            throw new InvalidOperationException("Prescricao nao encontrada.");
        }

        p.Status = "FINALIZADA";
        p.EndAt = DateTime.UtcNow;
        db.SaveChanges();
        return ToPrescriptionDto(p);
    }

    public List<ExamRequestDto> GetExams()
    {
        return db.ExamRequests.OrderByDescending(x => x.RequestedAt).Select(ToExamDto).ToList();
    }

    public ExamRequestDto CreateExam(CreateExamRequest request)
    {
        var admission = db.Admissions.FirstOrDefault(x => x.Id == request.AdmissionId);
        if (admission == null || admission.Status != "INTERNADO")
        {
            throw new InvalidOperationException("Exame exige internacao ativa.");
        }

        if (string.IsNullOrWhiteSpace(request.ExamType))
        {
            throw new InvalidOperationException("Tipo de exame obrigatorio.");
        }

        var exam = new ExamRequestRecord
        {
            Id = Guid.NewGuid(),
            AdmissionId = request.AdmissionId,
            ExamType = request.ExamType.Trim(),
            Status = "PENDENTE",
            RequestedAt = DateTime.UtcNow
        };

        db.ExamRequests.Add(exam);
        db.SaveChanges();
        return ToExamDto(exam);
    }

    public ExamRequestDto CompleteExam(Guid examId)
    {
        var exam = db.ExamRequests.FirstOrDefault(x => x.Id == examId);
        if (exam == null)
        {
            throw new InvalidOperationException("Exame nao encontrado.");
        }

        exam.Status = "CONCLUIDO";
        exam.CompletedAt = DateTime.UtcNow;
        db.SaveChanges();
        return ToExamDto(exam);
    }

    public List<InvoiceDto> GetInvoices()
    {
        return db.Invoices.OrderByDescending(x => x.CreatedAt).Select(ToInvoiceDto).ToList();
    }

    public List<AlertDto> GetAlerts()
    {
        var result = new List<AlertDto>();
        var beds = db.Beds.ToList();
        var occupied = beds.Count(x => x.Status == "OCUPADO");
        if (beds.Count > 0 && occupied >= Math.Ceiling(beds.Count * 0.6))
        {
            result.Add(new AlertDto("warning", "Leitos quase lotados", $"{occupied} de {beds.Count} leitos estao ocupados.", "leitos"));
        }

        var pendingExams = db.ExamRequests.Count(x => x.Status == "PENDENTE");
        if (pendingExams > 0)
        {
            result.Add(new AlertDto("danger", "Exames pendentes", $"{pendingExams} exame(s) aguardando conclusao.", "exames"));
        }

        var activePrescriptions = db.Prescriptions.Count(x => x.Status == "ATIVA");
        if (activePrescriptions > 0)
        {
            result.Add(new AlertDto("info", "Prescricoes ativas", $"{activePrescriptions} prescricao(oes) ainda ativas.", "prescricoes"));
        }

        var admissions = db.Admissions.Include(x => x.Patient).Include(x => x.Prescriptions).Include(x => x.Exams).Where(x => x.Status == "INTERNADO").ToList();
        foreach (var admission in admissions)
        {
            if (admission.Exams.Any(x => x.Status == "PENDENTE") || admission.Prescriptions.Any(x => x.Status == "ATIVA"))
            {
                result.Add(new AlertDto("warning", "Alta bloqueada", $"{admission.Patient?.FullName ?? "Paciente"} possui pendencias antes da alta.", "internacao"));
            }
        }

        return result;
    }

    private static AdmissionDto ToAdmissionDto(AdmissionRecord x)
    {
        return new AdmissionDto(x.Id, x.PatientId, x.Patient?.FullName ?? "", x.Bed?.Code ?? "", x.Status, x.AdmittedAt, x.DischargedAt);
    }

    private static PrescriptionDto ToPrescriptionDto(PrescriptionRecord x)
    {
        return new PrescriptionDto(x.Id, x.AdmissionId, x.MedicineName, x.Dose, x.FrequencyHours, x.StartAt, x.EndAt, x.Status);
    }

    private static ExamRequestDto ToExamDto(ExamRequestRecord x)
    {
        return new ExamRequestDto(x.Id, x.AdmissionId, x.ExamType, x.Status, x.RequestedAt, x.CompletedAt);
    }

    private static InvoiceDto ToInvoiceDto(InvoiceRecord x)
    {
        return new InvoiceDto(x.Id, x.AdmissionId, x.Amount, x.Status, x.CreatedAt);
    }
}
