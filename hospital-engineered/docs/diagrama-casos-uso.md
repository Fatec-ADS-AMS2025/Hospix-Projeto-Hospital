# Diagrama de casos de uso - hospital-engineered

Versao engineered: os casos de uso seguem o mesmo fluxo do legacy, mas estao alinhados a use cases de aplicacao, regras de dominio e fronteira externa de faturamento.

```mermaid
flowchart LR
    Medico["Ator: Medico"]
    Enfermagem["Ator: Enfermagem"]
    Administracao["Ator: Administracao"]
    Billing["Servico externo: BillingService"]

    subgraph Sistema["Sistema Hospitalar Engineered"]
        Dashboard([Consultar dashboard])
        GerarAlertas([Gerar alertas por regras])
        CadastrarPaciente([Cadastrar paciente])
        ConsultarLeitos([Consultar leitos])
        InternarPaciente([Internar paciente])
        CriarPrescricao([Criar prescricao])
        SolicitarExame([Solicitar exame])
        ConcluirPendencias([Concluir exame ou prescricao])
        DarAlta([Dar alta hospitalar])
        NotificarAlta([Notificar alta ao BillingService])
        ConsultarFaturas([Consultar faturas])
    end

    subgraph Faturamento["BillingService"]
        GerarFatura([Gerar fatura])
    end

    Medico --> Dashboard
    Medico --> ConsultarLeitos
    Medico --> CriarPrescricao
    Medico --> SolicitarExame

    Enfermagem --> InternarPaciente
    Enfermagem --> ConcluirPendencias
    Enfermagem --> DarAlta

    Administracao --> CadastrarPaciente
    Administracao --> ConsultarFaturas
    Administracao --> GerarAlertas

    DarAlta -. "<<include>> valida aggregate Admission" .-> ConcluirPendencias
    DarAlta -. "<<include>>" .-> NotificarAlta
    NotificarAlta -. "HTTP / evento de alta" .-> GerarFatura
    Billing --> GerarFatura
```

## Leitura do diagrama

- Os perfis aparecem na interface, mas ainda nao ha autorizacao real no backend.
- `Dar alta hospitalar` valida as regras no aggregate `Admission`.
- `Notificar alta ao BillingService` separa o contexto clinico do contexto de faturamento.
- `Gerar alertas por regras` representa o uso de estrategias `IAlertRule`.
