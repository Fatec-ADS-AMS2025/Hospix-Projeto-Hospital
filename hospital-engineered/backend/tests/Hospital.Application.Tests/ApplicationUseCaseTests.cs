using Hospital.Application.Alerts;
using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Application.UseCases;
using Hospital.Domain.Common;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.ValueObjects;

namespace Hospital.Application.Tests;

public class ApplicationUseCaseTests
{
    [Fact]
    public async Task CreatePrescriptionUseCase_creates_valid_prescription()
    {
        var store = DemoStore.WithActiveAdmission();
        var useCase = new CreatePrescriptionUseCase(store, store);

        var result = await useCase.ExecuteAsync(new CreatePrescriptionRequest(
            store.Admission.Id,
            "Cefalexina",
            "500mg",
            8,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(24)), CancellationToken.None);

        Assert.Equal("Cefalexina", result.MedicineName);
        Assert.Equal("ATIVA", result.Status);
        Assert.True(store.Saved);
    }

    [Fact]
    public async Task CreatePrescriptionUseCase_rejects_missing_or_inactive_admission()
    {
        var store = DemoStore.WithActiveAdmission();
        var useCase = new CreatePrescriptionUseCase(store, store);

        await Assert.ThrowsAsync<DomainException>(() => useCase.ExecuteAsync(new CreatePrescriptionRequest(
            Guid.NewGuid(),
            "Cefalexina",
            "500mg",
            8,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(24)), CancellationToken.None));

        store.Admission.Discharge(DateTime.UtcNow);

        await Assert.ThrowsAsync<DomainException>(() => useCase.ExecuteAsync(new CreatePrescriptionRequest(
            store.Admission.Id,
            "Cefalexina",
            "500mg",
            8,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(24)), CancellationToken.None));
    }

    [Fact]
    public async Task GenerateAlertsUseCase_returns_rule_based_alerts()
    {
        var store = DemoStore.WithActiveAdmission();
        store.Admission.RequestExam(Guid.NewGuid(), "Tomografia", DateTime.UtcNow);
        store.Admission.AddPrescription(Guid.NewGuid(), "Dipirona", new Dosage("500mg"), 6, new DateRange(DateTime.UtcNow, DateTime.UtcNow.AddHours(6)));
        var alerts = new GenerateAlertsUseCase(
            new IAlertRule[]
            {
                new BedOccupancyAlertRule(),
                new PendingExamAlertRule(),
                new ActivePrescriptionAlertRule(),
                new BlockedDischargeAlertRule()
            },
            store,
            store,
            store,
            store);

        var result = await alerts.ExecuteAsync(CancellationToken.None);

        Assert.Contains(result, x => x.Source == "exames");
        Assert.Contains(result, x => x.Source == "prescricoes");
        Assert.Contains(result, x => x.Source == "internacao");
    }

    private class DemoStore :
        IAdmissionRepository,
        IPatientRepository,
        IBedRepository,
        IPrescriptionRepository,
        IExamRepository,
        IUnitOfWork
    {
        private readonly List<Patient> patients = [];
        private readonly List<Bed> beds = [];
        private readonly List<Admission> admissions = [];

        private DemoStore()
        {
        }

        public Admission Admission => admissions.Single();
        public bool Saved { get; private set; }

        public static DemoStore WithActiveAdmission()
        {
            var store = new DemoStore();
            var patient = new Patient(Guid.NewGuid(), "Paciente App", new Cpf("12345678901"), new DateTime(1991, 1, 1), "Plano");
            var bed = new Bed(Guid.NewGuid(), "A-101", "Clinica");
            var admission = new Admission(Guid.NewGuid(), patient, bed, DateTime.UtcNow);
            store.patients.Add(patient);
            store.beds.Add(bed);
            store.admissions.Add(admission);
            return store;
        }

        Task<IReadOnlyList<Admission>> IAdmissionRepository.ListAsync(CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<Admission>>(admissions);
        Task<Admission?> IAdmissionRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken) => Task.FromResult(admissions.FirstOrDefault(x => x.Id == id));
        Task<Admission?> IAdmissionRepository.GetByPrescriptionIdAsync(Guid prescriptionId, CancellationToken cancellationToken) => Task.FromResult(admissions.FirstOrDefault(x => x.Prescriptions.Any(p => p.Id == prescriptionId)));
        Task<Admission?> IAdmissionRepository.GetByExamIdAsync(Guid examId, CancellationToken cancellationToken) => Task.FromResult(admissions.FirstOrDefault(x => x.Exams.Any(e => e.Id == examId)));
        Task<bool> IAdmissionRepository.HasActiveAdmissionAsync(Guid patientId, CancellationToken cancellationToken) => Task.FromResult(admissions.Any(x => x.PatientId == patientId && x.Status == AdmissionStatus.Active));
        Task IAdmissionRepository.AddAsync(Admission admission, CancellationToken cancellationToken)
        {
            admissions.Add(admission);
            return Task.CompletedTask;
        }

        Task<IReadOnlyList<Patient>> IPatientRepository.ListAsync(CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<Patient>>(patients);
        Task<Patient?> IPatientRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken) => Task.FromResult(patients.FirstOrDefault(x => x.Id == id));
        Task<Patient?> IPatientRepository.FindByCpfAsync(Cpf cpf, CancellationToken cancellationToken) => Task.FromResult(patients.FirstOrDefault(x => x.Cpf == cpf));
        Task IPatientRepository.AddAsync(Patient patient, CancellationToken cancellationToken)
        {
            patients.Add(patient);
            return Task.CompletedTask;
        }

        Task<IReadOnlyList<Bed>> IBedRepository.ListAsync(CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<Bed>>(beds);
        Task<Bed?> IBedRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken) => Task.FromResult(beds.FirstOrDefault(x => x.Id == id));
        Task<IReadOnlyList<Prescription>> IPrescriptionRepository.ListAsync(CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<Prescription>>(admissions.SelectMany(x => x.Prescriptions).ToList());
        Task<IReadOnlyList<ExamRequest>> IExamRepository.ListAsync(CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<ExamRequest>>(admissions.SelectMany(x => x.Exams).ToList());

        Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
        {
            Saved = true;
            return Task.CompletedTask;
        }
    }
}
