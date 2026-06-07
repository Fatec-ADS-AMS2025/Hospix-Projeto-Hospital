# Entrega - Grupo 1

Tema: Clean Code e SOLID Aplicados ao Modulo de Prescricao Medica.

Arquivos principais:

- `Grupo 1 - Clean Code e SOLID Prescricao Medica.docx`: trabalho escrito com 15 paginas, dentro da faixa exigida de 12 a 15 paginas, formatado em padrao ABNT/NBR 14724.
- `Grupo 1 - Seminario Clean Code e SOLID Prescricao.pptx`: apresentacao com 20 slides para a fala de 50 minutos, em visual mais limpo e unificado.
- `build_trabalho_grupo1.py`: script usado para gerar o DOCX.
- `build_slides_grupo1.py`: script usado para gerar os modulos do PPTX.

Validacoes registradas:

- `dotnet format .\Hospital.Engineered.slnx --verify-no-changes --verbosity minimal`: saiu com codigo 0.
- `dotnet test .\backend\tests\Hospital.Domain.Tests\Hospital.Domain.Tests.csproj`: 4/4 testes aprovados.
- `dotnet test .\backend\tests\Hospital.Application.Tests\Hospital.Application.Tests.csproj`: 3/3 testes aprovados.
- `dotnet test .\tests\HospitalLegacy.Api.Tests\HospitalLegacy.Api.Tests.csproj --no-build`: 2/2 testes aprovados.
- PPTX exportado com 20 slides e QA de layout sem erros.

Observacao: os nomes dos integrantes nao foram informados, entao o campo de integrantes no DOCX ficou marcado para preenchimento.
