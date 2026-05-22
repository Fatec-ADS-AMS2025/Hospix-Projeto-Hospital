# Problemas intencionais da versao legacy

Este repositorio existe para mostrar que "funcionar" nao significa ser sustentavel.

## Exemplos para apresentar

- `HospitalController` conhece todos os casos de uso do sistema.
- `HospitalService` mistura regras de pacientes, internacoes, prescricoes, exames, alertas e faturamento.
- Status sao strings soltas: `INTERNADO`, `ALTA`, `PENDENTE`, `ATIVA`.
- A regra de alta depende de buscas diretas no banco dentro do mesmo metodo que gera fatura.
- DTOs, requests e entidades ficam muito proximos, sem fronteira clara.
- O frontend concentra chamadas HTTP, estado, formularios e tabelas em `src/app/page.tsx`.
- Validacoes aparecem no backend e na tela sem um modelo central de dominio.

## Como conectar com o Tema 01

Use `HospitalService` para mostrar:

- metodo grande;
- duplicacao;
- acoplamento com EF Core;
- baixa testabilidade;
- nomes e responsabilidades pouco claros.

## Como conectar com o Tema 02

Use a internacao para mostrar que regras importantes estao espalhadas:

- paciente internado;
- leito disponivel;
- alta bloqueada;
- prescricoes e exames pendentes.

Sem DDD, essas regras nao aparecem como linguagem do negocio. Elas ficam escondidas em condicionais.

## Como conectar com o Tema 03

O sistema inteiro esta em um bloco:

- API unica;
- banco unico;
- faturamento dentro do mesmo service;
- nenhuma fronteira entre modulos.

Isso cria alto acoplamento e dificulta evolucao para microsservicos.
