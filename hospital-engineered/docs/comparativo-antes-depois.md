# Comparativo antes vs depois

| Aspecto | hospital-legacy | hospital-engineered |
|---|---|---|
| Codigo | Service grande | Use cases pequenos |
| Regra de negocio | Condicionais espalhadas | Entidades e value objects |
| Prescricao | Metodo dentro de service geral | `CreatePrescriptionUseCase` |
| Internacao | Modelo anemico | Aggregate root `Admission` |
| Faturamento | Mesmo service | BillingService separado |
| Alertas | Metodo unico | Estrategias `IAlertRule` |
| Frontend | Uma pagina grande | Feature + shared components |
| Testes | Smoke e fluxo feliz | Domain, Application e API |

## Mensagem para a apresentacao

O objetivo nao e mostrar que a versao legacy esta "errada" por nao funcionar. Ela funciona. O problema e que ela e dificil de manter. A versao engineered mostra como a engenharia de software organiza a mesma complexidade.
