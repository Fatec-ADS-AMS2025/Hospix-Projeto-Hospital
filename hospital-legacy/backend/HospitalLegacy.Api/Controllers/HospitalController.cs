using HospitalLegacy.Api.Models;
using HospitalLegacy.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalLegacy.Api.Controllers;

[ApiController]
[Route("api")]
public class HospitalController : ControllerBase
{
    private readonly HospitalService service;

    public HospitalController(HospitalService service)
    {
        this.service = service;
    }

    [HttpGet("dashboard")]
    public ActionResult<DashboardDto> Dashboard()
    {
        return service.GetDashboard();
    }

    [HttpGet("patients")]
    public ActionResult<List<PatientDto>> Patients()
    {
        return service.GetPatients();
    }

    [HttpPost("patients")]
    public ActionResult<PatientDto> CreatePatient(CreatePatientRequest request)
    {
        try
        {
            return service.CreatePatient(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("beds")]
    public ActionResult<List<BedDto>> Beds()
    {
        return service.GetBeds();
    }

    [HttpGet("admissions")]
    public ActionResult<List<AdmissionDto>> Admissions()
    {
        return service.GetAdmissions();
    }

    [HttpPost("admissions")]
    public ActionResult<AdmissionDto> CreateAdmission(CreateAdmissionRequest request)
    {
        try
        {
            return service.CreateAdmission(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("admissions/{id:guid}/discharge")]
    public ActionResult<AdmissionDto> Discharge(Guid id)
    {
        try
        {
            return service.Discharge(id);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("prescriptions")]
    public ActionResult<List<PrescriptionDto>> Prescriptions()
    {
        return service.GetPrescriptions();
    }

    [HttpPost("prescriptions")]
    public ActionResult<PrescriptionDto> CreatePrescription(CreatePrescriptionRequest request)
    {
        try
        {
            return service.CreatePrescription(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("prescriptions/{id:guid}/complete")]
    public ActionResult<PrescriptionDto> CompletePrescription(Guid id)
    {
        try
        {
            return service.CompletePrescription(id);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("exams")]
    public ActionResult<List<ExamRequestDto>> Exams()
    {
        return service.GetExams();
    }

    [HttpPost("exams")]
    public ActionResult<ExamRequestDto> CreateExam(CreateExamRequest request)
    {
        try
        {
            return service.CreateExam(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("exams/{id:guid}/complete")]
    public ActionResult<ExamRequestDto> CompleteExam(Guid id)
    {
        try
        {
            return service.CompleteExam(id);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("alerts")]
    public ActionResult<List<AlertDto>> Alerts()
    {
        return service.GetAlerts();
    }

    [HttpGet("invoices")]
    public ActionResult<List<InvoiceDto>> Invoices()
    {
        return service.GetInvoices();
    }
}
