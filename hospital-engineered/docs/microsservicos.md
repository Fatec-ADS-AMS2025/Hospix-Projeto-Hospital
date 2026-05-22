# Microsservicos

## Decisao do projeto

O seminario usa uma arquitetura hibrida leve:

- API principal modular e organizada;
- BillingService separado para demonstrar comunicacao entre servicos;
- SQLite separado para faturamento.

## Sequencia de alta e faturamento

```mermaid
sequenceDiagram
  participant UI as Next.js
  participant API as Hospital.Api
  participant APP as AdmissionUseCases
  participant DB as SQLite Clinico
  participant BILL as BillingService
  participant BDB as SQLite Faturamento

  UI->>API: PATCH /api/admissions/{id}/discharge
  API->>APP: DischargeAsync(id)
  APP->>DB: carrega internacao
  APP->>APP: valida pendencias
  APP->>DB: salva alta e libera leito
  APP->>BILL: POST /api/billing/admission-discharged
  BILL->>BDB: grava fatura
  BILL-->>APP: fatura gerada
  APP-->>API: AdmissionDto
  API-->>UI: alta concluida
```

## Tradeoffs

- Vantagem: fronteira clara entre clinico e faturamento.
- Vantagem: demonstra banco por servico.
- Limite: comunicacao e sincronizacao ainda sao simples.
- Limite: nao ha mensageria real para manter o escopo adequado ao seminario.
