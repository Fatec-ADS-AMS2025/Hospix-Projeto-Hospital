# Roteiro de demo - hospital-legacy

## Cena 1: sistema funcionando

Abrir o frontend em `http://localhost:3001` e mostrar dashboard, alertas, formularios e tabelas.

Mensagem central: o sistema entrega valor, mas isso nao prova que ele foi bem projetado.

## Cena 2: fluxo feliz

1. Cadastrar um paciente.
2. Selecionar um leito disponivel.
3. Criar internacao.
4. Criar prescricao.
5. Solicitar exame.
6. Finalizar prescricao.
7. Concluir exame.
8. Dar alta.
9. Mostrar fatura gerada.

## Cena 3: problema de manutencao

Abrir `backend/HospitalLegacy.Api/Services/HospitalService.cs`.

Pontos para comentar:

- Um service sabe demais.
- Controller apenas repassa chamadas, mas todos os modulos dependem da mesma classe.
- Regra de alta e faturamento estao no mesmo metodo.
- Se o hospital mudar a regra de alta, existe risco de quebrar faturamento.

## Cena 4: frontend acoplado

Abrir `frontend/src/app/page.tsx`.

Pontos para comentar:

- Estado de todos os modulos no mesmo componente.
- Forms, tabelas, fetch e transformacoes misturados.
- Dificil reaproveitar em outro modulo.

## Fechamento

Concluir que este repositorio e o exemplo de partida para justificar Clean Code, SOLID, DDD e Clean Architecture.
