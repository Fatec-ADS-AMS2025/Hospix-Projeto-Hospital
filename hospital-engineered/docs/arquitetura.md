# Arquitetura

## Visao geral

O `hospital-engineered` organiza o sistema em camadas e modulos. A API principal concentra os fluxos clinicos e envia um evento HTTP para o `Hospital.BillingService` quando uma alta e concluida.

```mermaid
flowchart LR
  Frontend["Next.js Frontend"]
  Api["Hospital.Api"]
  App["Hospital.Application"]
  Domain["Hospital.Domain"]
  Infra["Hospital.Infrastructure"]
  Db[("SQLite Clinico")]
  Billing["Hospital.BillingService"]
  BillingDb[("SQLite Faturamento")]

  Frontend --> Api
  Api --> App
  App --> Domain
  Infra --> App
  Infra --> Domain
  Api --> Infra
  Infra --> Db
  App --> Billing
  Billing --> BillingDb
```

## Dependencias

- `Hospital.Domain` nao depende de nenhum projeto.
- `Hospital.Application` depende de `Hospital.Domain`.
- `Hospital.Infrastructure` depende de `Hospital.Application` e `Hospital.Domain`.
- `Hospital.Api` depende de `Hospital.Application` e `Hospital.Infrastructure`.
- `Hospital.BillingService` e separado e possui banco proprio.

## Clean Architecture

```mermaid
flowchart TB
  Api["API / Controllers"]
  UseCases["Application / Use Cases"]
  Domain["Domain / Entidades e Value Objects"]
  Infrastructure["Infrastructure / EF Core, Repositorios, Gateway HTTP"]

  Api --> UseCases
  UseCases --> Domain
  Infrastructure --> UseCases
  Infrastructure --> Domain
```
