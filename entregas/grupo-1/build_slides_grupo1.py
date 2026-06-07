from __future__ import annotations

from pathlib import Path
import textwrap


ROOT = Path(__file__).resolve().parents[2]
WORKSPACE = ROOT / "outputs" / "manual-grupo-1" / "presentations" / "clean-code-solid-prescricao"
SLIDES = WORKSPACE / "slides"
OUT = ROOT / "entregas" / "grupo-1" / "Grupo 1 - Seminario Clean Code e SOLID Prescricao.pptx"


THEME = r"""
export const palette = {
  bg: "#FFFFFF",
  ink: "#111827",
  muted: "#4B5563",
  line: "#D1D5DB",
  panel: "#FFFFFF",
  blue: "#1F4E79",
  teal: "#0F5F5C",
  green: "#0F5F5C",
  amber: "#6B7280",
  red: "#374151",
  purple: "#374151",
  slate: "#374151",
  lightBlue: "#F3F6FA",
  lightTeal: "#F1F7F6",
  lightAmber: "#F7F7F4",
  lightRed: "#F7F7F7",
  lightGreen: "#F2F7F5",
  code: "#111827",
  codeBg: "#F3F4F6"
};

export function base(slide, ctx, section = "Grupo 1 - Qualidade do codigo") {
  ctx.addShape(slide, { x: 0, y: 0, w: 1280, h: 720, fill: palette.bg });
  ctx.addShape(slide, { x: 0, y: 0, w: 9, h: 720, fill: palette.teal });
  ctx.addShape(slide, { x: 38, y: 665, w: 1160, h: 1, fill: palette.line, line: ctx.line(palette.line, 0) });
  ctx.addText(slide, {
    x: 40, y: 684, w: 760, h: 22,
    text: section,
    fontSize: 14, color: palette.muted, typeface: ctx.fonts.body
  });
  ctx.addText(slide, {
    x: 1120, y: 684, w: 120, h: 22,
    text: `slide ${String(ctx.slideNumber).padStart(2, "0")}`,
    fontSize: 14, color: palette.muted, align: "right", typeface: ctx.fonts.body
  });
}

export function title(slide, ctx, text, kicker = "") {
  if (kicker) {
    ctx.addText(slide, {
      x: 48, y: 36, w: 900, h: 24,
      text: kicker.toUpperCase(),
      fontSize: 15, bold: true, color: palette.teal, typeface: ctx.fonts.body
    });
  }
  ctx.addText(slide, {
    x: 48, y: kicker ? 65 : 45, w: 1060, h: 78,
    text,
    fontSize: 34, bold: true, color: palette.ink, typeface: ctx.fonts.title,
    insets: { left: 0, right: 0, top: 0, bottom: 0 }
  });
}

export function subtitle(slide, ctx, text, y = 155, w = 1020) {
  ctx.addText(slide, {
    x: 50, y, w, h: 54,
    text,
    fontSize: 20, color: palette.muted, typeface: ctx.fonts.body,
    insets: { left: 0, right: 0, top: 0, bottom: 0 }
  });
}

export function panel(slide, ctx, { x, y, w, h, fill = palette.panel, line = palette.line, accent, titleText, body, titleColor = palette.ink, bodySize = 18 }) {
  ctx.addShape(slide, { x, y, w, h, fill, line: ctx.line(line, 1) });
  if (accent) ctx.addShape(slide, { x, y, w: 7, h, fill: accent, line: ctx.line(accent, 0) });
  if (titleText) {
    ctx.addText(slide, {
      x: x + 18, y: y + 14, w: w - 34, h: 30,
      text: titleText,
      fontSize: 19, bold: true, color: titleColor, typeface: ctx.fonts.body
    });
  }
  if (body) {
    ctx.addText(slide, {
      x: x + 18, y: y + (titleText ? 50 : 18), w: w - 34, h: h - (titleText ? 62 : 30),
      text: body,
      fontSize: bodySize, color: palette.slate, typeface: ctx.fonts.body,
      insets: { left: 0, right: 0, top: 0, bottom: 0 }
    });
  }
}

export function label(slide, ctx, { x, y, w, text, fill = palette.lightBlue, color = palette.blue }) {
  ctx.addShape(slide, { x, y, w, h: 30, fill, line: ctx.line(fill, 0) });
  ctx.addText(slide, {
    x: x + 10, y: y + 5, w: w - 20, h: 22,
    text,
    fontSize: 14, bold: true, color, align: "center", typeface: ctx.fonts.body
  });
}

export function code(slide, ctx, { x, y, w, h, text, titleText = "codigo", color = palette.blue, fontSize = 13 }) {
  ctx.addShape(slide, { x, y, w, h, fill: palette.codeBg, line: ctx.line("#CBD5E1", 1) });
  ctx.addShape(slide, { x, y, w, h: 30, fill: color, line: ctx.line(color, 0) });
  ctx.addText(slide, {
    x: x + 12, y: y + 7, w: w - 24, h: 20,
    text: titleText,
    fontSize: 13, bold: true, color: "#FFFFFF", typeface: ctx.fonts.body
  });
  ctx.addText(slide, {
    x: x + 14, y: y + 42, w: w - 28, h: h - 48,
    text,
    fontSize, color: palette.code, typeface: ctx.fonts.mono,
    insets: { left: 0, right: 0, top: 0, bottom: 0 }
  });
}

export function flow(slide, ctx, items, y, color = palette.blue) {
  const gap = 22;
  const w = Math.floor((1120 - gap * (items.length - 1)) / items.length);
  let x = 60;
  items.forEach((item, index) => {
    ctx.addShape(slide, { x, y, w, h: 88, fill: palette.panel, line: ctx.line(color, 1.2) });
    ctx.addText(slide, {
      x: x + 14, y: y + 16, w: w - 28, h: 58,
      text: item,
      fontSize: 18, bold: true, color: palette.ink, align: "center", valign: "middle", typeface: ctx.fonts.body
    });
    if (index < items.length - 1) {
      ctx.addShape(slide, { x: x + w + 4, y: y + 41, w: gap - 8, h: 6, fill: color, line: ctx.line(color, 0) });
    }
    x += w + gap;
  });
}

export function bullets(slide, ctx, { x, y, w, h, items, fontSize = 20, color = palette.slate }) {
  const text = items.map((item) => `- ${item}`).join("\n");
  ctx.addText(slide, {
    x, y, w, h, text, fontSize, color, typeface: ctx.fonts.body,
    insets: { left: 0, right: 0, top: 0, bottom: 0 }
  });
}

export function metric(slide, ctx, { x, y, w, value, labelText, fill = palette.lightGreen, color = palette.green }) {
  ctx.addShape(slide, { x, y, w, h: 110, fill, line: ctx.line(fill, 0) });
  ctx.addText(slide, { x: x + 12, y: y + 18, w: w - 24, h: 42, text: value, fontSize: 34, bold: true, color, align: "center", typeface: ctx.fonts.title });
  ctx.addText(slide, { x: x + 14, y: y + 65, w: w - 28, h: 32, text: labelText, fontSize: 16, color: palette.slate, align: "center", typeface: ctx.fonts.body });
}
"""


SLIDES_DATA = [
    {
        "title": "Clean Code e SOLID no modulo de prescricao medica",
        "kicker": "Seminario de Engenharia de Software",
        "kind": "cover",
        "panels": [
            ("Pergunta norteadora", "Como escrever codigo sustentavel em sistemas hospitalares complexos?", "teal"),
            ("Recorte do grupo", "Cadastro de prescricao, medicamentos, horarios, validacao e separacao de responsabilidades.", "blue"),
            ("Entrega", "Trabalho escrito 12-15 paginas + apresentacao de 50 minutos.", "amber"),
        ],
    },
    {
        "title": "A apresentacao segue a estrutura obrigatoria da atividade",
        "kicker": "Roteiro de 50 minutos",
        "kind": "agenda",
    },
    {
        "title": "O Grupo 1 olha o mesmo hospital pela lente da qualidade do codigo",
        "kicker": "Contexto comum",
        "subtitle": "Os tres grupos usam o mesmo sistema: pacientes, internacoes, prescricoes, exames, faturamento e comunicacao entre setores.",
        "kind": "scope",
    },
    {
        "title": "No legado, prescricao fica dentro de um service que faz quase tudo",
        "kicker": "Antes",
        "kind": "legacy-map",
    },
    {
        "title": "O problema nao e so tamanho: e mistura de decisao, banco e resposta",
        "kicker": "Codigo legado",
        "kind": "legacy-code",
    },
    {
        "title": "Em hospital, codigo confuso vira custo de manutencao e risco operacional",
        "kicker": "Impacto",
        "kind": "impact",
    },
    {
        "title": "Clean Code transforma a prescricao em um fluxo legivel",
        "kicker": "Fundamentacao",
        "kind": "clean-code",
    },
    {
        "title": "SOLID foi aplicado onde ele resolve problemas reais do projeto",
        "kicker": "SRP, OCP e DIP",
        "kind": "solid",
    },
    {
        "title": "A refatoracao preserva o comportamento e muda a organizacao",
        "kicker": "Antes -> Depois",
        "kind": "refactor-plan",
    },
    {
        "title": "No engineered, o caso de uso orquestra e o dominio decide",
        "kicker": "Depois",
        "kind": "engineered-flow",
    },
    {
        "title": "O caso de uso fica curto porque delega regras para objetos certos",
        "kicker": "Codigo depois",
        "kind": "engineered-code",
    },
    {
        "title": "Dose e periodo foram separados porque carregam regras do dominio",
        "kicker": "Value objects",
        "kind": "value-objects",
    },
    {
        "title": "SRP: cada classe passa a ter um motivo principal para mudar",
        "kicker": "SOLID aplicado",
        "kind": "srp",
    },
    {
        "title": "DIP: o caso de uso depende de contratos, nao de EF Core",
        "kicker": "SOLID aplicado",
        "kind": "dip",
    },
    {
        "title": "OCP aparece nas regras de alerta, sem editar o orquestrador",
        "kicker": "SOLID aplicado",
        "kind": "ocp",
    },
    {
        "title": "A entrega usa ferramenta de analise e testes como evidencia",
        "kicker": "Validacao",
        "kind": "evidence",
    },
    {
        "title": "Comparacao final: o engineered custa mais arquivos, mas reduz custo de mudanca",
        "kicker": "Antes vs depois",
        "kind": "compare",
    },
    {
        "title": "Roteiro de demonstracao para a fala do grupo",
        "kicker": "Apresentacao",
        "kind": "demo",
    },
    {
        "title": "A continuidade entre os grupos fica clara pelo mesmo dominio",
        "kicker": "Integracao",
        "kind": "groups",
    },
    {
        "title": "Codigo sustentavel deixa regra clinica explicita, testavel e evolutiva",
        "kicker": "Conclusao",
        "kind": "conclusion",
    },
]


def js_string(value: str) -> str:
    return value.replace("\\", "\\\\").replace("`", "\\`").replace("${", "\\${")


def slide_module(index: int, spec: dict) -> str:
    kind = spec["kind"]
    title = js_string(spec["title"])
    kicker = js_string(spec.get("kicker", ""))
    subtitle = js_string(spec.get("subtitle", ""))
    body = render_kind(kind, spec)
    return f"""import {{ base, title, subtitle, panel, label, code, flow, bullets, metric, palette }} from "./_theme.mjs";

export async function slide{index:02d}(presentation, ctx) {{
  const slide = presentation.slides.add();
  base(slide, ctx);
  title(slide, ctx, `{title}`, `{kicker}`);
{f"  subtitle(slide, ctx, `{subtitle}`);" if subtitle else ""}
{body}
  return slide;
}}
"""


def render_kind(kind: str, spec: dict) -> str:
    if kind == "cover":
        panels = []
        y = 320
        colors = {"teal": "palette.teal", "blue": "palette.blue", "amber": "palette.amber"}
        fills = {"teal": "palette.lightTeal", "blue": "palette.lightBlue", "amber": "palette.lightAmber"}
        for i, (t, b, c) in enumerate(spec["panels"]):
            panels.append(f"""  panel(slide, ctx, {{ x: {70 + i * 390}, y: {y}, w: 350, h: 150, fill: {fills[c]}, accent: {colors[c]}, titleText: `{js_string(t)}`, body: `{js_string(b)}`, bodySize: 18 }});""")
        return "\n".join([
            "  ctx.addShape(slide, { x: 50, y: 210, w: 1140, h: 3, fill: palette.teal, line: ctx.line(palette.teal, 0) });",
            "  subtitle(slide, ctx, `Grupo 1 - Qualidade do codigo | Sistema Hospitalar Inteligente`, 235, 900);",
            *panels,
        ])
    if kind == "agenda":
        return """
  flow(slide, ctx, [`Introducao\\n10 min`, `Teoria\\n15 min`, `Aplicacao\\n20 min`, `Conclusao\\n5 min`], 220, palette.teal);
  panel(slide, ctx, { x: 90, y: 370, w: 500, h: 130, accent: palette.blue, titleText: `Entrega escrita`, body: `12 a 15 paginas com introducao, fundamentacao, aplicacao pratica, codigo/modelagem, discussao, conclusao e referencias.`, bodySize: 18 });
  panel(slide, ctx, { x: 690, y: 370, w: 500, h: 130, accent: palette.amber, titleText: `Entrega oral`, body: `Mostrar codigo, explicar refatoracao, comparar antes vs depois e responder a pergunta do Tema 01.`, bodySize: 18 });
"""
    if kind == "scope":
        return """
  panel(slide, ctx, { x: 70, y: 250, w: 340, h: 190, fill: palette.lightBlue, accent: palette.blue, titleText: `Grupo 1`, body: `Qualidade do codigo\\nClean Code\\nSOLID\\nRefatoracao`, bodySize: 21 });
  panel(slide, ctx, { x: 460, y: 250, w: 340, h: 190, fill: palette.lightAmber, accent: palette.amber, titleText: `Grupo 2`, body: `Regras de negocio\\nPoliticas de internacao\\nAlta e faturamento`, bodySize: 21 });
  panel(slide, ctx, { x: 850, y: 250, w: 340, h: 190, fill: palette.lightTeal, accent: palette.teal, titleText: `Grupo 3`, body: `Arquitetura\\nCamadas\\nDominio e infraestrutura`, bodySize: 21 });
  label(slide, ctx, { x: 345, y: 500, w: 590, text: `Mesmo vocabulario: paciente, internacao, prescricao, exame, leito e fatura`, fill: palette.lightGreen, color: palette.green });
"""
    if kind == "legacy-map":
        return """
  panel(slide, ctx, { x: 70, y: 185, w: 260, h: 110, accent: palette.slate, titleText: `HospitalController`, body: `Recebe HTTP e chama service`, bodySize: 17 });
  panel(slide, ctx, { x: 460, y: 160, w: 360, h: 230, fill: palette.lightRed, accent: palette.red, titleText: `HospitalService`, body: `Dashboard\\nPacientes\\nInternacoes\\nPrescricoes\\nExames\\nAlta\\nAlertas\\nFaturamento`, bodySize: 19 });
  panel(slide, ctx, { x: 950, y: 185, w: 240, h: 110, accent: palette.slate, titleText: `HospitalDbContext`, body: `DbSet e SaveChanges direto`, bodySize: 17 });
  ctx.addShape(slide, { x: 335, y: 238, w: 120, h: 8, fill: palette.red, line: ctx.line(palette.red, 0) });
  ctx.addShape(slide, { x: 825, y: 238, w: 120, h: 8, fill: palette.red, line: ctx.line(palette.red, 0) });
  bullets(slide, ctx, { x: 95, y: 470, w: 1080, h: 120, items: [`Classe com varios motivos para mudar`, `Regra de prescricao misturada com persistencia`, `Strings de status e validacoes espalhadas`], fontSize: 22 });
"""
    if kind == "legacy-code":
        return """
  code(slide, ctx, { x: 55, y: 170, w: 735, h: 420, titleText: `HospitalService.CreatePrescription`, color: palette.red, fontSize: 12, text: `if (string.IsNullOrWhiteSpace(request.MedicineName) ||\\n    string.IsNullOrWhiteSpace(request.Dose))\\n    throw new InvalidOperationException(...);\\n\\nif (request.FrequencyHours <= 0)\\n    throw new InvalidOperationException(...);\\n\\nif (request.EndAt <= request.StartAt)\\n    throw new InvalidOperationException(...);\\n\\nvar admission = db.Admissions.FirstOrDefault(...);\\nif (admission == null || admission.Status != \"INTERNADO\")\\n    throw new InvalidOperationException(...);\\n\\ndb.Prescriptions.Add(prescription);\\ndb.SaveChanges();` });
  panel(slide, ctx, { x: 835, y: 180, w: 350, h: 92, fill: palette.lightRed, accent: palette.red, titleText: `Problema 1`, body: `Validacao, consulta ao banco, criacao e persistencia no mesmo fluxo.`, bodySize: 17 });
  panel(slide, ctx, { x: 835, y: 295, w: 350, h: 92, fill: palette.lightAmber, accent: palette.amber, titleText: `Problema 2`, body: `Status como string: \"INTERNADO\" e \"ATIVA\".`, bodySize: 17 });
  panel(slide, ctx, { x: 835, y: 410, w: 350, h: 92, fill: palette.lightBlue, accent: palette.blue, titleText: `Problema 3`, body: `Teste isolado fica mais dificil por depender do DbContext direto.`, bodySize: 17 });
"""
    if kind == "impact":
        return """
  metric(slide, ctx, { x: 80, y: 215, w: 250, value: `1 regra`, labelText: `pode afetar alta, alerta e faturamento`, fill: palette.lightAmber, color: palette.amber });
  metric(slide, ctx, { x: 385, y: 215, w: 250, value: `1 service`, labelText: `vira ponto unico de mudanca`, fill: palette.lightRed, color: palette.red });
  metric(slide, ctx, { x: 690, y: 215, w: 250, value: `+ risco`, labelText: `quando regra fica implicita`, fill: palette.lightBlue, color: palette.blue });
  metric(slide, ctx, { x: 995, y: 215, w: 180, value: `- clareza`, labelText: `para manutencao`, fill: palette.lightTeal, color: palette.teal });
  panel(slide, ctx, { x: 130, y: 405, w: 1020, h: 120, accent: palette.teal, titleText: `Traducao para o dominio hospitalar`, body: `Prescricao nao e so uma linha no banco. Ela influencia alertas, bloqueio de alta, historico clinico e cobranca. Por isso, regra importante precisa aparecer no codigo de forma explicita.`, bodySize: 21 });
"""
    if kind == "clean-code":
        return """
  panel(slide, ctx, { x: 75, y: 175, w: 350, h: 155, fill: palette.lightBlue, accent: palette.blue, titleText: `Nomes significativos`, body: `CreatePrescriptionUseCase, Admission, Prescription, Dosage e DateRange usam termos do hospital.`, bodySize: 18 });
  panel(slide, ctx, { x: 465, y: 175, w: 350, h: 155, fill: palette.lightGreen, accent: palette.green, titleText: `Metodos curtos`, body: `O fluxo principal fica legivel: buscar internacao, adicionar prescricao, salvar e retornar DTO.`, bodySize: 18 });
  panel(slide, ctx, { x: 855, y: 175, w: 350, h: 155, fill: palette.lightTeal, accent: palette.teal, titleText: `Organizacao`, body: `Validacao invariavel fica no dominio; persistencia fica na infraestrutura.`, bodySize: 18 });
  panel(slide, ctx, { x: 205, y: 395, w: 870, h: 110, accent: palette.purple, titleText: `Resultado`, body: `O codigo deixa de obrigar o leitor a descobrir a regra por varias condicionais e passa a contar a historia do caso de uso.`, bodySize: 22 });
"""
    if kind == "solid":
        return """
  panel(slide, ctx, { x: 80, y: 200, w: 330, h: 220, fill: palette.lightBlue, accent: palette.blue, titleText: `SRP`, body: `CreatePrescriptionUseCase cria prescricao.\\nPrescription valida prescricao.\\nDosage valida dose.`, bodySize: 21 });
  panel(slide, ctx, { x: 470, y: 200, w: 330, h: 220, fill: palette.lightTeal, accent: palette.teal, titleText: `DIP`, body: `Application depende de IAdmissionRepository e IUnitOfWork.\\nEF Core fica fora do caso de uso.`, bodySize: 21 });
  panel(slide, ctx, { x: 860, y: 200, w: 330, h: 220, fill: palette.lightAmber, accent: palette.amber, titleText: `OCP`, body: `Novas regras de alerta entram por IAlertRule, sem alterar o orquestrador.`, bodySize: 21 });
  label(slide, ctx, { x: 300, y: 490, w: 680, text: `Apenas os principios aplicados ao projeto, como pedido no Tema 01`, fill: palette.lightGreen, color: palette.green });
"""
    if kind == "refactor-plan":
        return """
  flow(slide, ctx, [`1. Isolar\\ncaso de uso`, `2. Mover\\nvalidacoes`, `3. Depender de\\ninterfaces`, `4. Testar\\ncomportamento`], 205, palette.blue);
  panel(slide, ctx, { x: 110, y: 390, w: 480, h: 130, fill: palette.lightRed, accent: palette.red, titleText: `Antes`, body: `HospitalService cria prescricao, consulta banco, valida dados e salva diretamente.`, bodySize: 20 });
  panel(slide, ctx, { x: 690, y: 390, w: 480, h: 130, fill: palette.lightGreen, accent: palette.green, titleText: `Depois`, body: `Use case orquestra; Admission, Prescription, Dosage e DateRange protegem regras.`, bodySize: 20 });
"""
    if kind == "engineered-flow":
        return """
  flow(slide, ctx, [`Prescriptions\\nController`, `CreatePrescription\\nUseCase`, `Admission\\nAggregate`, `Prescription +\\nValue Objects`, `UnitOfWork +\\nRepository`], 235, palette.teal);
  panel(slide, ctx, { x: 115, y: 410, w: 1030, h: 105, accent: palette.blue, titleText: `Leitura da arquitetura no ponto do Grupo 1`, body: `A separacao nao existe por enfeite: ela deixa claro quem recebe HTTP, quem orquestra, quem valida regra e quem persiste.`, bodySize: 22 });
"""
    if kind == "engineered-code":
        return """
  code(slide, ctx, { x: 65, y: 170, w: 760, h: 420, titleText: `CreatePrescriptionUseCase.ExecuteAsync`, color: palette.teal, fontSize: 13, text: `var admission = await admissions.GetByIdAsync(request.AdmissionId, ct)\\n    ?? throw new DomainException(\"Internacao nao encontrada.\");\\n\\nvar prescription = admission.AddPrescription(\\n    Guid.NewGuid(),\\n    request.MedicineName,\\n    new Dosage(request.Dose),\\n    request.FrequencyHours,\\n    new DateRange(request.StartAt, request.EndAt));\\n\\nawait unitOfWork.SaveChangesAsync(ct);\\nreturn HospitalMapping.ToDto(prescription);` });
  panel(slide, ctx, { x: 865, y: 185, w: 330, h: 105, fill: palette.lightTeal, accent: palette.teal, titleText: `Orquestracao`, body: `O use case mostra o fluxo, nao todos os detalhes.`, bodySize: 18 });
  panel(slide, ctx, { x: 865, y: 320, w: 330, h: 105, fill: palette.lightBlue, accent: palette.blue, titleText: `Dominio`, body: `Admission.AddPrescription valida internacao ativa.`, bodySize: 18 });
  panel(slide, ctx, { x: 865, y: 455, w: 330, h: 105, fill: palette.lightGreen, accent: palette.green, titleText: `Persistencia`, body: `Salvar e responsabilidade do Unit of Work.`, bodySize: 18 });
"""
    if kind == "value-objects":
        return """
  code(slide, ctx, { x: 70, y: 180, w: 510, h: 260, titleText: `Dosage`, color: palette.blue, fontSize: 13, text: `public Dosage(string value)\\n{\\n    if (string.IsNullOrWhiteSpace(value))\\n        throw new DomainException(\"Dose e obrigatoria.\");\\n\\n    Value = value.Trim();\\n}` });
  code(slide, ctx, { x: 670, y: 180, w: 510, h: 260, titleText: `DateRange`, color: palette.teal, fontSize: 13, text: `public DateRange(DateTime startAt, DateTime endAt)\\n{\\n    if (endAt <= startAt)\\n        throw new DomainException(\"Data final deve ser posterior...\");\\n\\n    StartAt = startAt;\\n    EndAt = endAt;\\n}` });
  panel(slide, ctx, { x: 170, y: 500, w: 940, h: 88, accent: palette.amber, titleText: `Por que separar?`, body: `Porque dose e periodo nao sao dados soltos: eles impedem estados invalidos em qualquer ponto que tente criar uma prescricao.`, bodySize: 20 });
"""
    if kind == "srp":
        return """
  panel(slide, ctx, { x: 65, y: 190, w: 260, h: 135, accent: palette.red, fill: palette.lightRed, titleText: `Legado`, body: `HospitalService muda por dashboard, paciente, leito, internacao, prescricao, exame, alta e faturamento.`, bodySize: 17 });
  panel(slide, ctx, { x: 390, y: 160, w: 260, h: 115, accent: palette.blue, titleText: `Use case`, body: `Muda quando muda o fluxo de criar prescricao.`, bodySize: 18 });
  panel(slide, ctx, { x: 705, y: 280, w: 260, h: 115, accent: palette.teal, titleText: `Dominio`, body: `Muda quando muda regra da prescricao.`, bodySize: 18 });
  panel(slide, ctx, { x: 390, y: 430, w: 260, h: 115, accent: palette.green, titleText: `Value objects`, body: `Mudam quando muda dose ou periodo.`, bodySize: 18 });
  ctx.addShape(slide, { x: 330, y: 250, w: 60, h: 6, fill: palette.blue, line: ctx.line(palette.blue, 0) });
  ctx.addShape(slide, { x: 650, y: 330, w: 55, h: 6, fill: palette.teal, line: ctx.line(palette.teal, 0) });
  ctx.addShape(slide, { x: 510, y: 390, w: 6, h: 40, fill: palette.green, line: ctx.line(palette.green, 0) });
  panel(slide, ctx, { x: 1010, y: 220, w: 180, h: 200, fill: palette.lightGreen, accent: palette.green, titleText: `Ganho`, body: `Menos efeito colateral e mais facilidade para localizar regra.`, bodySize: 19 });
"""
    if kind == "dip":
        return """
  panel(slide, ctx, { x: 80, y: 205, w: 300, h: 165, fill: palette.lightTeal, accent: palette.teal, titleText: `Application`, body: `CreatePrescriptionUseCase depende de contratos.`, bodySize: 21 });
  panel(slide, ctx, { x: 490, y: 150, w: 300, h: 120, fill: palette.lightBlue, accent: palette.blue, titleText: `IAdmissionRepository`, body: `Busca internacao por id.`, bodySize: 20 });
  panel(slide, ctx, { x: 490, y: 350, w: 300, h: 120, fill: palette.lightBlue, accent: palette.blue, titleText: `IUnitOfWork`, body: `Salva alteracoes.`, bodySize: 20 });
  panel(slide, ctx, { x: 900, y: 205, w: 300, h: 165, fill: palette.lightAmber, accent: palette.amber, titleText: `Infrastructure`, body: `EfHospitalRepository implementa os contratos usando EF Core.`, bodySize: 20 });
  ctx.addShape(slide, { x: 382, y: 245, w: 105, h: 6, fill: palette.teal, line: ctx.line(palette.teal, 0) });
  ctx.addShape(slide, { x: 382, y: 315, w: 105, h: 6, fill: palette.teal, line: ctx.line(palette.teal, 0) });
  ctx.addShape(slide, { x: 792, y: 245, w: 105, h: 6, fill: palette.amber, line: ctx.line(palette.amber, 0) });
  ctx.addShape(slide, { x: 792, y: 315, w: 105, h: 6, fill: palette.amber, line: ctx.line(palette.amber, 0) });
  label(slide, ctx, { x: 260, y: 530, w: 760, text: `O caso de uso pode ser testado com fake repository, sem banco real`, fill: palette.lightGreen, color: palette.green });
"""
    if kind == "ocp":
        return """
  code(slide, ctx, { x: 65, y: 170, w: 520, h: 300, titleText: `Contrato`, color: palette.purple, fontSize: 14, text: `public interface IAlertRule\\n{\\n    IEnumerable<AlertDto> Evaluate(\\n        HospitalAlertContext context);\\n}` });
  panel(slide, ctx, { x: 665, y: 160, w: 260, h: 105, fill: palette.lightBlue, accent: palette.blue, titleText: `PendingExamAlertRule`, body: `Alerta para exames pendentes.`, bodySize: 17 });
  panel(slide, ctx, { x: 960, y: 160, w: 260, h: 105, fill: palette.lightTeal, accent: palette.teal, titleText: `ActivePrescriptionAlertRule`, body: `Alerta para prescricoes ativas.`, bodySize: 17 });
  panel(slide, ctx, { x: 665, y: 310, w: 260, h: 105, fill: palette.lightAmber, accent: palette.amber, titleText: `BlockedDischargeAlertRule`, body: `Alerta de alta bloqueada.`, bodySize: 17 });
  panel(slide, ctx, { x: 960, y: 310, w: 260, h: 105, fill: palette.lightGreen, accent: palette.green, titleText: `Nova regra`, body: `Entra como nova classe, sem reabrir o orquestrador.`, bodySize: 17 });
  panel(slide, ctx, { x: 190, y: 520, w: 900, h: 80, accent: palette.purple, titleText: `Observacao`, body: `OCP aparece em funcionalidade ligada a prescricao: alertas baseados em prescricoes ativas.`, bodySize: 19 });
"""
    if kind == "evidence":
        return """
  metric(slide, ctx, { x: 95, y: 190, w: 250, value: `0`, labelText: `falhas no dotnet format`, fill: palette.lightGreen, color: palette.green });
  metric(slide, ctx, { x: 395, y: 190, w: 250, value: `4/4`, labelText: `testes de dominio`, fill: palette.lightBlue, color: palette.blue });
  metric(slide, ctx, { x: 695, y: 190, w: 250, value: `3/3`, labelText: `testes de aplicacao`, fill: palette.lightTeal, color: palette.teal });
  metric(slide, ctx, { x: 995, y: 190, w: 170, value: `2/2`, labelText: `testes legado`, fill: palette.lightAmber, color: palette.amber });
  code(slide, ctx, { x: 155, y: 390, w: 970, h: 160, titleText: `Comandos usados como evidencia`, color: palette.green, fontSize: 15, text: `dotnet format .\\\\Hospital.Engineered.slnx --verify-no-changes\\ndotnet test .\\\\backend\\\\tests\\\\Hospital.Domain.Tests\\\\Hospital.Domain.Tests.csproj\\ndotnet test .\\\\backend\\\\tests\\\\Hospital.Application.Tests\\\\Hospital.Application.Tests.csproj\\ndotnet test .\\\\tests\\\\HospitalLegacy.Api.Tests\\\\HospitalLegacy.Api.Tests.csproj --no-build` });
"""
    if kind == "compare":
        return """
  panel(slide, ctx, { x: 70, y: 175, w: 525, h: 390, fill: palette.lightRed, accent: palette.red, titleText: `Legado`, body: `- Service geral\\n- DbContext direto\\n- Status em string\\n- Validacao no fluxo\\n- Baixa testabilidade isolada\\n- Simples de desenhar, caro de manter`, bodySize: 22 });
  panel(slide, ctx, { x: 685, y: 175, w: 525, h: 390, fill: palette.lightGreen, accent: palette.green, titleText: `Engineered`, body: `- Caso de uso especifico\\n- Interfaces de repositorio\\n- Enums e value objects\\n- Regras no dominio\\n- Teste com fake repository\\n- Mais arquivos, menor custo de mudanca`, bodySize: 22 });
  ctx.addText(slide, { x: 610, y: 338, w: 60, h: 40, text: `vs`, fontSize: 26, bold: true, color: palette.muted, align: "center", typeface: ctx.fonts.title });
"""
    if kind == "demo":
        return """
  panel(slide, ctx, { x: 80, y: 170, w: 260, h: 360, accent: palette.blue, titleText: `1. Mostrar legado`, body: `Abrir HospitalService.CreatePrescription e apontar mistura de responsabilidades.`, bodySize: 20 });
  panel(slide, ctx, { x: 365, y: 170, w: 260, h: 360, accent: palette.teal, titleText: `2. Mostrar engineered`, body: `Abrir CreatePrescriptionUseCase e Admission.AddPrescription.`, bodySize: 20 });
  panel(slide, ctx, { x: 650, y: 170, w: 260, h: 360, accent: palette.green, titleText: `3. Mostrar validacao`, body: `Abrir Dosage, DateRange e teste CreatePrescriptionUseCase_creates_valid_prescription.`, bodySize: 20 });
  panel(slide, ctx, { x: 935, y: 170, w: 260, h: 360, accent: palette.amber, titleText: `4. Fechar comparacao`, body: `Explicar por que a solucao e coerente para sistema hospitalar complexo.`, bodySize: 20 });
"""
    if kind == "groups":
        return """
  panel(slide, ctx, { x: 75, y: 190, w: 340, h: 230, fill: palette.lightBlue, accent: palette.blue, titleText: `Grupo 1`, body: `Codigo sustentavel\\nClean Code\\nSOLID\\nRefatoracao`, bodySize: 23 });
  panel(slide, ctx, { x: 465, y: 190, w: 340, h: 230, fill: palette.lightAmber, accent: palette.amber, titleText: `Grupo 2`, body: `Regras de negocio\\nInternacao ativa\\nAlta bloqueada\\nFaturamento`, bodySize: 23 });
  panel(slide, ctx, { x: 855, y: 190, w: 340, h: 230, fill: palette.lightTeal, accent: palette.teal, titleText: `Grupo 3`, body: `Arquitetura\\nCamadas\\nRepositorios\\nServicos externos`, bodySize: 23 });
  label(slide, ctx, { x: 250, y: 500, w: 780, text: `Continuidade: codigo bom sustenta regras; arquitetura organiza codigo e dominio`, fill: palette.lightGreen, color: palette.green });
"""
    if kind == "conclusion":
        return """
  panel(slide, ctx, { x: 105, y: 175, w: 980, h: 120, fill: palette.lightGreen, accent: palette.green, titleText: `Resposta ao Tema 01`, body: `Codigo sustentavel em hospital nasce quando regra importante fica explicita no dominio, fluxo de aplicacao fica curto, dependencias apontam para abstracoes e comportamento e validado por testes.`, bodySize: 23 });
  flow(slide, ctx, [`Clareza`, `Responsabilidade`, `Abstracao`, `Validacao`], 360, palette.green);
  subtitle(slide, ctx, `Perguntas`, 535, 600);
  ctx.addShape(slide, { x: 930, y: 500, w: 210, h: 4, fill: palette.teal, line: ctx.line(palette.teal, 0) });
"""
    raise ValueError(kind)


def main() -> None:
    SLIDES.mkdir(parents=True, exist_ok=True)
    (WORKSPACE / "preview").mkdir(parents=True, exist_ok=True)
    (WORKSPACE / "layout").mkdir(parents=True, exist_ok=True)
    (WORKSPACE / "qa").mkdir(parents=True, exist_ok=True)
    (WORKSPACE / "output").mkdir(parents=True, exist_ok=True)
    (WORKSPACE / "profile-plan.txt").write_text(
        textwrap.dedent(
            """\
            task mode: create
            primary deck-profile: engineering-platform
            proof objects: before/after code, dependency flow, SOLID mapping, validation evidence
            asset requirements: native editable shapes and code snippets; no external identity assets
            QA gates: 20 slides, legible labels, no repeated macro layout for more than three slides, exported PPTX and preview contact sheet
            known missing inputs: nomes dos integrantes nao informados
            """
        ),
        encoding="utf-8",
    )
    (WORKSPACE / "source-notes.txt").write_text(
        textwrap.dedent(
            """\
            Sources:
            - Tema 01.docx: Clean Code e SOLID Aplicados ao Modulo de Prescricao Medica.
            - hospital-legacy/backend/HospitalLegacy.Api/Services/HospitalService.cs.
            - hospital-engineered/backend/src/Hospital.Application/UseCases/CreatePrescriptionUseCase.cs.
            - hospital-engineered/backend/src/Hospital.Domain/Entities/Admission.cs.
            - hospital-engineered/backend/src/Hospital.Domain/ValueObjects/Dosage.cs.
            - hospital-engineered/backend/src/Hospital.Domain/ValueObjects/DateRange.cs.
            - Test command outputs captured in the Codex run.
            """
        ),
        encoding="utf-8",
    )
    (SLIDES / "_theme.mjs").write_text(THEME, encoding="utf-8")
    for index, spec in enumerate(SLIDES_DATA, start=1):
        (SLIDES / f"slide-{index:02d}.mjs").write_text(slide_module(index, spec), encoding="utf-8")
    print(f"slides_dir={SLIDES}")
    print(f"out={OUT}")


if __name__ == "__main__":
    main()
