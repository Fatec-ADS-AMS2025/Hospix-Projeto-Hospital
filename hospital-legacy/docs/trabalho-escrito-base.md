# Base do trabalho escrito - versao legacy

## Introducao

O repositorio `hospital-legacy` representa uma versao inicial de um sistema hospitalar web para pacientes, internacoes, prescricoes, exames e faturamento. A aplicacao foi construida para funcionar, mas sem aplicar conscientemente principios de engenharia de software.

## Discussao tecnica

A implementacao mostra problemas recorrentes em sistemas corporativos:

- controllers e services grandes;
- regras de negocio misturadas com infraestrutura;
- modelos sem comportamento;
- ausencia de linguagem ubiqua;
- baixa separacao entre modulos;
- frontend com logica de interface e de integracao misturadas.

## Aplicacao no seminario

Este repositorio deve ser usado como evidencia do problema. Ele permite mostrar:

- por que Clean Code melhora legibilidade;
- por que SOLID reduz acoplamento;
- por que DDD torna regras de negocio explicitas;
- por que arquitetura separa responsabilidades;
- por que microsservicos exigem fronteiras claras antes de distribuicao.

## Conclusao parcial

O `hospital-legacy` mostra que um sistema pode atender um fluxo de negocio e ainda assim ser fragil para manutencao, crescimento e evolucao tecnica.
