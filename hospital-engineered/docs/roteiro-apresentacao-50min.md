# Roteiro de apresentacao - 50 minutos

## 0-10 min: Introducao

- Contexto: sistema hospitalar web.
- Fluxo comum: paciente, internacao, prescricao, exame, alta e faturamento.
- Dois repositorios: legacy e engineered.

## 10-25 min: Conceitos teoricos

- Clean Code: legibilidade, nomes, metodos curtos.
- SOLID: SRP, OCP, DIP.
- DDD: linguagem ubiqua, entidade, value object, aggregate, bounded context.
- Clean Architecture: separacao entre dominio, casos de uso, infraestrutura e API.
- Microsservicos: fronteiras, comunicacao e banco por servico.

## 25-45 min: Aplicacao pratica

- Abrir `hospital-legacy` e executar o fluxo.
- Mostrar `HospitalService` como exemplo de acoplamento.
- Abrir `hospital-engineered` e executar o mesmo fluxo.
- Mostrar `Admission`, `CreatePrescriptionUseCase`, `IAlertRule` e `HttpBillingGateway`.
- Mostrar diagramas de arquitetura e sequencia.

## 45-50 min: Conclusao

- Engenharia de software nao e so programar.
- Codigo, dominio e arquitetura precisam conversar.
- Sistemas hospitalares exigem organizacao em multiplas camadas.
