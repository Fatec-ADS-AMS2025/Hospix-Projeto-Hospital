# Diagrama de classes - hospital-legacy

Versao legacy: controller, service, DbContext e records ficam ligados diretamente. O foco do diagrama e mostrar a concentracao de responsabilidades no `HospitalService`.

```mermaid
classDiagram
    direction TB

    class HospitalController {
        -HospitalService service
        +Dashboard() IActionResult
        +Patients()
        +Beds()
        +Admissions()
        +Prescriptions()
        +Exams()
        +Invoices()
    }

    class HospitalService {
        -HospitalDbContext db
        +GetDashboard()
        +CreatePatient(request)
        +CreateAdmission(request)
        +CreatePrescription(request)
        +CreateExam(request)
        +CompleteExam(id)
        +CompletePrescription(id)
        +Discharge(id)
        +GetAlerts()
        +GetInvoices()
    }

    class HospitalDbContext {
        +DbSet~PatientRecord~ Patients
        +DbSet~BedRecord~ Beds
        +DbSet~AdmissionRecord~ Admissions
        +DbSet~PrescriptionRecord~ Prescriptions
        +DbSet~ExamRequestRecord~ ExamRequests
        +DbSet~InvoiceRecord~ Invoices
    }

    class PatientRecord {
        +Guid Id
        +string FullName
        +string Cpf
        +DateTime BirthDate
        +string InsuranceName
    }

    class BedRecord {
        +Guid Id
        +string Code
        +string Ward
        +string Status
    }

    class AdmissionRecord {
        +Guid Id
        +Guid PatientId
        +Guid BedId
        +string Status
        +DateTime AdmittedAt
        +DateTime? DischargedAt
    }

    class PrescriptionRecord {
        +Guid Id
        +Guid AdmissionId
        +string MedicineName
        +string Dose
        +int FrequencyHours
        +DateTime StartAt
        +DateTime EndAt
        +string Status
    }

    class ExamRequestRecord {
        +Guid Id
        +Guid AdmissionId
        +string ExamType
        +string Status
        +DateTime RequestedAt
        +DateTime? CompletedAt
    }

    class InvoiceRecord {
        +Guid Id
        +Guid AdmissionId
        +decimal Amount
        +string Status
        +DateTime CreatedAt
    }

    HospitalController --> HospitalService : usa
    HospitalService --> HospitalDbContext : acessa EF Core
    HospitalDbContext "1" --> "*" PatientRecord
    HospitalDbContext "1" --> "*" BedRecord
    HospitalDbContext "1" --> "*" AdmissionRecord
    HospitalDbContext "1" --> "*" PrescriptionRecord
    HospitalDbContext "1" --> "*" ExamRequestRecord
    HospitalDbContext "1" --> "*" InvoiceRecord
    AdmissionRecord "*" --> "1" PatientRecord : patient
    AdmissionRecord "*" --> "1" BedRecord : bed
    AdmissionRecord "1" *-- "*" PrescriptionRecord : prescriptions
    AdmissionRecord "1" *-- "*" ExamRequestRecord : exams
    AdmissionRecord "1" --> "0..1" InvoiceRecord : invoice
```

## Leitura do diagrama

- O `HospitalService` concentra pacientes, internacoes, prescricoes, exames, alertas e faturamento.
- Os `Record` sao usados como modelo de banco e base para resposta da API.
- Os status sao strings soltas, como `INTERNADO`, `ALTA`, `PENDENTE` e `ATIVA`.
- A regra de alta e a geracao de fatura ficam no mesmo metodo de service.
