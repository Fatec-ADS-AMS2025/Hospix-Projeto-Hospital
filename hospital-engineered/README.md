# hospital-engineered

Versao organizada do Sistema Hospitalar Inteligente, aplicando Clean Code, SOLID, DDD, Clean Architecture e um microsservico leve de faturamento.

## Como rodar

BillingService:

```powershell
cd backend/src/Hospital.BillingService
dotnet run --urls http://localhost:5102
```

API principal:

```powershell
cd backend/src/Hospital.Api
dotnet run --urls http://localhost:5101
```

Frontend:

```powershell
cd frontend
npm run dev -- --port 3101
```

URLs:

- API principal: http://localhost:5101
- Swagger API: http://localhost:5101/swagger
- BillingService: http://localhost:5102
- Swagger Billing: http://localhost:5102/swagger
- Frontend: http://localhost:3101

## Fluxo da demo

1. Cadastrar paciente.
2. Internar paciente em leito disponivel.
3. Criar prescricao.
4. Solicitar exame.
5. Tentar alta e observar bloqueios.
6. Finalizar prescricao e concluir exame.
7. Dar alta.
8. Ver fatura gerada pelo BillingService.

## Diagrama HTML

- `docs/diagrama-arquitetura.html`
- `docs/diagrama-classes.html`
- `docs/diagrama-casos-uso.html`

## Diagramas Markdown

- `docs/diagrama-classes.md`
- `docs/diagrama-casos-uso.md`

## Reset dos bancos

Pare os servicos e remova:

- `backend/src/Hospital.Api/hospital-engineered.db`
- `backend/src/Hospital.BillingService/hospital-billing.db`

## Testes

```powershell
dotnet test
npm run lint --prefix frontend
```
