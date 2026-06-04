# Diagrama de casos de uso - hospital-legacy

Versao legacy: os casos de uso existem no produto, mas ficam implementados de forma centralizada no `HospitalService`.

```mermaid
flowchart LR
    Medico["Ator: Medico"]
    Enfermagem["Ator: Enfermagem"]
    Administracao["Ator: Administracao"]

    subgraph Sistema["Sistema Hospitalar Legacy"]
        Dashboard([Consultar dashboard])
        Alertas([Consultar alertas])
        CadastrarPaciente([Cadastrar paciente])
        ConsultarLeitos([Consultar leitos])
        InternarPaciente([Internar paciente])
        CriarPrescricao([Criar prescricao])
        SolicitarExame([Solicitar exame])
        ConcluirPendencias([Concluir exame ou prescricao])
        DarAlta([Dar alta hospitalar])
        GerarFatura([Gerar fatura no mesmo service])
        ConsultarFaturas([Consultar faturas])
    end

    Medico --> Dashboard
    Medico --> ConsultarLeitos
    Medico --> CriarPrescricao

    Enfermagem --> InternarPaciente
    Enfermagem --> SolicitarExame
    Enfermagem --> ConcluirPendencias
    Enfermagem --> DarAlta

    Administracao --> CadastrarPaciente
    Administracao --> ConsultarFaturas
    Administracao --> Alertas

    DarAlta -. "<<include>> valida pendencias" .-> ConcluirPendencias
    DarAlta -. "<<include>>" .-> GerarFatura
```

## Leitura do diagrama

- Os atores acessam o mesmo sistema sem controle real de permissao.
- A alta inclui validar pendencias e gerar fatura.
- No legacy, esses casos nao viram classes de aplicacao separadas; eles ficam dentro do `HospitalService`.
