# Diagrama de classes - hospital-engineered

Versao engineered: o dominio possui entidades com comportamento, use cases orquestram o fluxo e interfaces isolam infraestrutura e faturamento.

```mermaid
classDiagram
    direction TB

    class Patient {
        +Guid Id
        +string FullName
        +Cpf Cpf
        +DateTime BirthDate
        +string InsuranceName
    }

    class Cpf {
        <<Value Object>>
        +string Value
    }

    class Bed {
        +Guid Id
        +string Code
        +string Ward
        +BedStatus Status
        +Occupy()
        +Release()
    }

    class Admission {
        <<Aggregate Root>>
        +Guid Id
        +Guid PatientId
        +Guid BedId
        +AdmissionStatus Status
        +DateTime AdmittedAt
        +DateTime? DischargedAt
        +AddPrescription()
        +RequestExam()
        +CompletePrescription()
        +CompleteExam()
        +Discharge()
    }

    class Prescription {
        +Guid Id
        +Guid AdmissionId
        +string MedicineName
        +Dosage Dose
        +DateRange Period
        +PrescriptionStatus Status
        +Complete()
    }

    class ExamRequest {
        +Guid Id
        +Guid AdmissionId
        +string ExamType
        +ExamStatus Status
        +DateTime RequestedAt
        +DateTime? CompletedAt
        +Complete()
    }

    class Dosage {
        <<Value Object>>
        +string Value
    }

    class DateRange {
        <<Value Object>>
        +DateTime StartAt
        +DateTime EndAt
        +double TotalHours
    }

    class Money {
        <<Value Object>>
        +decimal Amount
    }

    class CreatePrescriptionUseCase {
        -IAdmissionRepository admissions
        -IUnitOfWork unitOfWork
        +ExecuteAsync(command)
    }

    class AdmissionUseCases {
        -IAdmissionRepository admissions
        -IPatientRepository patients
        -IBedRepository beds
        -IInvoiceGateway invoices
        -IUnitOfWork unitOfWork
        +CreateAsync(request)
        +DischargeAsync(id)
    }

    class IAdmissionRepository {
        <<interface>>
        +GetByIdAsync(id)
        +HasActiveAdmissionAsync(patientId)
        +AddAsync(admission)
    }

    class IPatientRepository {
        <<interface>>
        +ListAsync()
        +GetByIdAsync(id)
        +FindByCpfAsync(cpf)
        +AddAsync(patient)
    }

    class IBedRepository {
        <<interface>>
        +ListAsync()
        +GetByIdAsync(id)
    }

    class IUnitOfWork {
        <<interface>>
        +SaveChangesAsync()
    }

    class IInvoiceGateway {
        <<interface>>
        +NotifyAdmissionDischargedAsync(event)
        +ListInvoicesAsync()
    }

    Patient "1" --> "*" Admission : patient
    Bed "1" --> "*" Admission : bed
    Patient --> Cpf : value object
    Admission "1" *-- "0..*" Prescription : prescriptions
    Admission "1" *-- "0..*" ExamRequest : exams
    Prescription --> Dosage : dose
    Prescription --> DateRange : period
    AdmissionUseCases --> Admission : orquestra
    AdmissionUseCases ..> IAdmissionRepository : depende
    AdmissionUseCases ..> IPatientRepository : depende
    AdmissionUseCases ..> IBedRepository : depende
    AdmissionUseCases ..> IInvoiceGateway : notifica alta
    AdmissionUseCases ..> IUnitOfWork : salva
    CreatePrescriptionUseCase --> Prescription : cria
    CreatePrescriptionUseCase ..> IAdmissionRepository : depende
    CreatePrescriptionUseCase ..> IUnitOfWork : salva
    IInvoiceGateway ..> Money : fatura
```

## Leitura do diagrama

- `Admission` e o aggregate root do fluxo de internacao.
- A alta e bloqueada quando existem exames pendentes ou prescricoes ativas.
- `Cpf`, `Dosage`, `DateRange` e `Money` sao value objects.
- Os use cases dependem de interfaces, nao diretamente de EF Core, SQLite ou HTTP.
