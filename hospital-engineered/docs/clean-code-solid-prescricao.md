# Clean Code e SOLID na prescricao

## Antes

No `hospital-legacy`, a prescricao e criada dentro de `HospitalService`, junto com internacao, exame, alta, alerta e faturamento.

Problemas:

- metodo grande;
- responsabilidade misturada;
- dependencia direta de EF Core;
- regra de negocio escondida em condicionais;
- baixa testabilidade.

## Depois

No `hospital-engineered`, a criacao de prescricao fica em `CreatePrescriptionUseCase`.

Decisoes:

- nomes especificos;
- entrada tipada por `CreatePrescriptionRequest`;
- validacao de periodo em `DateRange`;
- validacao de dose em `Dosage`;
- dependencia de abstracao `IAdmissionRepository`;
- persistencia atras de `IUnitOfWork`.

## SOLID

- SRP: `CreatePrescriptionUseCase` cria prescricao, nao processa alta nem faturamento.
- OCP: novas regras de alerta entram por `IAlertRule`, sem editar o use case principal.
- DIP: Application depende de interfaces, Infrastructure fornece EF Core e HTTP.
