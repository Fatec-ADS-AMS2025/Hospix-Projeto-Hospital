# hospital-legacy

Versao funcional, mas propositalmente mal estruturada, do Sistema Hospitalar Inteligente.

## Objetivo

Mostrar uma implementacao que funciona, porem concentra responsabilidades, duplica validacoes e mistura regras de negocio, persistencia, controller e UI. Este repositorio serve como o "antes" da comparacao do seminario.

## Como rodar

Backend:

```powershell
cd backend/HospitalLegacy.Api
dotnet run --urls http://localhost:5001
```

Frontend:

```powershell
cd frontend
npm run dev -- --port 3001
```

URLs:

- API: http://localhost:5001
- Swagger: http://localhost:5001/swagger
- Frontend: http://localhost:3001

## Fluxo da demo

1. Cadastrar paciente.
2. Internar paciente em um leito disponivel.
3. Criar prescricao.
4. Solicitar exame.
5. Tentar dar alta e observar bloqueio por pendencias.
6. Finalizar prescricao e concluir exame.
7. Dar alta e verificar fatura gerada.

## Diagrama HTML

- `docs/diagrama-arquitetura.html`
- `docs/diagrama-classes.html`
- `docs/diagrama-casos-uso.html`

## Diagramas Markdown

- `docs/diagrama-classes.md`
- `docs/diagrama-casos-uso.md`

## Reset do banco

Pare a API e remova o arquivo `backend/HospitalLegacy.Api/hospital-legacy.db`.

## Testes

```powershell
dotnet test
```
