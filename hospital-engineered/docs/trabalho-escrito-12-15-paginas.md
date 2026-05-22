# Trabalho escrito - base 12 a 15 paginas

## 1. Introducao

Este trabalho apresenta o desenvolvimento de um sistema hospitalar inteligente para gerenciar pacientes, internacoes, prescricoes, exames, faturamento e comunicacao entre setores. O objetivo e comparar uma implementacao funcional sem organizacao arquitetural com uma versao orientada por principios de engenharia de software.

## 2. Fundamentacao teorica

Abordar:

- Clean Code;
- SOLID;
- Domain-Driven Design;
- Clean Architecture;
- microsservicos;
- testes automatizados.

Referencias principais:

- Microsoft Learn - ASP.NET Core Web API e OpenAPI.
- Microsoft Learn - EF Core.
- Next.js App Router docs.
- Robert C. Martin - Clean Code e Clean Architecture.
- Eric Evans - Domain-Driven Design.

## 3. Aplicacao pratica

O fluxo implementado e:

`Paciente -> Internacao -> Prescricao -> Exame -> Alta -> Faturamento -> Alertas`

Esse fluxo conecta os tres temas do seminario:

- qualidade do codigo na prescricao;
- regras de negocio na internacao;
- arquitetura no sistema geral.

## 4. Diagramas, codigo e modelagem

Incluir os diagramas de `arquitetura.md`, `ddd-internacao.md` e `microsservicos.md`.

Trechos sugeridos:

- `hospital-legacy/backend/HospitalLegacy.Api/Services/HospitalService.cs`;
- `hospital-engineered/backend/src/Hospital.Domain/Entities/Admission.cs`;
- `hospital-engineered/backend/src/Hospital.Application/UseCases/CreatePrescriptionUseCase.cs`;
- `hospital-engineered/backend/src/Hospital.Application/Alerts/IAlertRule.cs`.

## 5. Discussao

A versao legacy reduz barreiras iniciais, mas aumenta acoplamento. A versao engineered exige mais estrutura, mas melhora testabilidade, manutencao e comunicacao entre grupos.

## 6. Conclusao

O projeto mostra que sistemas hospitalares complexos precisam de organizacao em codigo, dominio e arquitetura. A engenharia de software aparece como disciplina para sustentar evolucao, nao apenas como escrita de codigo.
