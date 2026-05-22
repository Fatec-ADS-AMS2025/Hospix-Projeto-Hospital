"use client";

import {
  AlertTriangle,
  BedDouble,
  ClipboardList,
  FlaskConical,
  ReceiptText,
  RefreshCw,
  Stethoscope,
  UserPlus,
} from "lucide-react";
import { FormEvent, useEffect, useMemo, useState } from "react";

const API = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5001/api";

type PatientDto = {
  id: string;
  fullName: string;
  cpf: string;
  birthDate: string;
  insuranceName: string;
};

type BedDto = {
  id: string;
  code: string;
  ward: string;
  status: string;
};

type AdmissionDto = {
  id: string;
  patientId: string;
  patientName: string;
  bedCode: string;
  status: string;
  admittedAt: string;
  dischargedAt: string | null;
};

type PrescriptionDto = {
  id: string;
  admissionId: string;
  medicineName: string;
  dose: string;
  frequencyHours: number;
  startAt: string;
  endAt: string;
  status: string;
};

type ExamRequestDto = {
  id: string;
  admissionId: string;
  examType: string;
  status: string;
  requestedAt: string;
  completedAt: string | null;
};

type InvoiceDto = {
  id: string;
  admissionId: string;
  amount: number;
  status: string;
  createdAt: string;
};

type AlertDto = {
  level: string;
  title: string;
  message: string;
  source: string;
};

type DashboardDto = {
  patients: number;
  activeAdmissions: number;
  occupiedBeds: number;
  pendingExams: number;
  activePrescriptions: number;
  invoices: number;
  admissions: AdmissionDto[];
  alerts: AlertDto[];
};

type PatientForm = {
  fullName: string;
  cpf: string;
  birthDate: string;
  insuranceName: string;
};

type PrescriptionForm = {
  admissionId: string;
  medicineName: string;
  dose: string;
  frequencyHours: string;
  durationHours: string;
};

async function request<T>(route: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${API}${route}`, {
    ...init,
    headers: {
      "Content-Type": "application/json",
      ...(init?.headers ?? {}),
    },
  });

  if (!response.ok) {
    const body = await response.text();
    throw new Error(body || "Erro inesperado na API legacy.");
  }

  return response.json() as Promise<T>;
}

export default function Home() {
  const [dashboard, setDashboard] = useState<DashboardDto | null>(null);
  const [patients, setPatients] = useState<PatientDto[]>([]);
  const [beds, setBeds] = useState<BedDto[]>([]);
  const [admissions, setAdmissions] = useState<AdmissionDto[]>([]);
  const [prescriptions, setPrescriptions] = useState<PrescriptionDto[]>([]);
  const [exams, setExams] = useState<ExamRequestDto[]>([]);
  const [invoices, setInvoices] = useState<InvoiceDto[]>([]);
  const [patientForm, setPatientForm] = useState<PatientForm>({
    fullName: "Joao Seminario",
    cpf: "98765432100",
    birthDate: "1994-05-10",
    insuranceName: "Plano Aula",
  });
  const [admissionPatientId, setAdmissionPatientId] = useState("");
  const [admissionBedId, setAdmissionBedId] = useState("");
  const [prescriptionForm, setPrescriptionForm] = useState<PrescriptionForm>({
    admissionId: "",
    medicineName: "Cefalexina",
    dose: "500mg",
    frequencyHours: "8",
    durationHours: "24",
  });
  const [examAdmissionId, setExamAdmissionId] = useState("");
  const [examType, setExamType] = useState("Tomografia");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);

  const activeAdmissions = useMemo(
    () => admissions.filter((admission) => admission.status === "INTERNADO"),
    [admissions],
  );

  async function loadEverything() {
    setLoading(true);
    setMessage("");
    try {
      const [nextDashboard, nextPatients, nextBeds, nextAdmissions, nextPrescriptions, nextExams, nextInvoices] =
        await Promise.all([
          request<DashboardDto>("/dashboard"),
          request<PatientDto[]>("/patients"),
          request<BedDto[]>("/beds"),
          request<AdmissionDto[]>("/admissions"),
          request<PrescriptionDto[]>("/prescriptions"),
          request<ExamRequestDto[]>("/exams"),
          request<InvoiceDto[]>("/invoices"),
        ]);

      setDashboard(nextDashboard);
      setPatients(nextPatients);
      setBeds(nextBeds);
      setAdmissions(nextAdmissions);
      setPrescriptions(nextPrescriptions);
      setExams(nextExams);
      setInvoices(nextInvoices);
      setAdmissionPatientId(nextPatients[0]?.id ?? "");
      setAdmissionBedId(nextBeds.find((bed) => bed.status === "DISPONIVEL")?.id ?? "");
      setPrescriptionForm((current) => ({ ...current, admissionId: nextAdmissions.find((x) => x.status === "INTERNADO")?.id ?? "" }));
      setExamAdmissionId(nextAdmissions.find((x) => x.status === "INTERNADO")?.id ?? "");
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "Falha ao carregar dados.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    const loadTimer = window.setTimeout(() => {
      void loadEverything();
    }, 0);

    return () => window.clearTimeout(loadTimer);
  }, []);

  async function createPatient(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    try {
      await request<PatientDto>("/patients", {
        method: "POST",
        body: JSON.stringify(patientForm),
      });
      setMessage("Paciente criado na versao legacy.");
      await loadEverything();
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "Erro ao criar paciente.");
    }
  }

  async function createAdmission(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    try {
      await request<AdmissionDto>("/admissions", {
        method: "POST",
        body: JSON.stringify({ patientId: admissionPatientId, bedId: admissionBedId }),
      });
      setMessage("Internacao criada com regra dentro do service gigante.");
      await loadEverything();
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "Erro ao internar.");
    }
  }

  async function createPrescription(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const startAt = new Date();
    const endAt = new Date(startAt.getTime() + Number(prescriptionForm.durationHours) * 60 * 60 * 1000);
    try {
      await request<PrescriptionDto>("/prescriptions", {
        method: "POST",
        body: JSON.stringify({
          admissionId: prescriptionForm.admissionId,
          medicineName: prescriptionForm.medicineName,
          dose: prescriptionForm.dose,
          frequencyHours: Number(prescriptionForm.frequencyHours),
          startAt: startAt.toISOString(),
          endAt: endAt.toISOString(),
        }),
      });
      setMessage("Prescricao criada com validacoes espalhadas.");
      await loadEverything();
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "Erro ao prescrever.");
    }
  }

  async function createExam(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    try {
      await request<ExamRequestDto>("/exams", {
        method: "POST",
        body: JSON.stringify({ admissionId: examAdmissionId, examType }),
      });
      setMessage("Exame solicitado.");
      await loadEverything();
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "Erro ao solicitar exame.");
    }
  }

  async function patch(route: string, text: string) {
    try {
      await fetch(`${API}${route}`, { method: "PATCH" }).then(async (response) => {
        if (!response.ok) {
          throw new Error(await response.text());
        }
      });
      setMessage(text);
      await loadEverything();
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "Operacao recusada.");
    }
  }

  return (
    <main className="min-h-screen bg-[#f4f7f6] text-slate-950">
      <div className="mx-auto flex max-w-7xl flex-col gap-6 px-4 py-5 sm:px-6 lg:px-8">
        <header className="flex flex-col gap-4 border-b border-slate-300 pb-5 md:flex-row md:items-center md:justify-between">
          <div>
            <p className="text-sm font-semibold uppercase tracking-[0.18em] text-red-700">hospital-legacy</p>
            <h1 className="text-3xl font-semibold tracking-normal">Sistema Hospitalar Inteligente</h1>
          </div>
          <button
            className="inline-flex h-10 items-center justify-center gap-2 rounded-md bg-slate-950 px-4 text-sm font-semibold text-white hover:bg-slate-800"
            onClick={() => void loadEverything()}
            type="button"
          >
            <RefreshCw size={16} />
            Atualizar
          </button>
        </header>

        {message ? <div className="rounded-md border border-amber-300 bg-amber-50 px-4 py-3 text-sm text-amber-950">{message}</div> : null}

        <section className="grid gap-3 sm:grid-cols-2 lg:grid-cols-6">
          <Metric icon={<UserPlus size={18} />} label="Pacientes" value={dashboard?.patients ?? 0} />
          <Metric icon={<Stethoscope size={18} />} label="Internados" value={dashboard?.activeAdmissions ?? 0} />
          <Metric icon={<BedDouble size={18} />} label="Leitos ocupados" value={dashboard?.occupiedBeds ?? 0} />
          <Metric icon={<FlaskConical size={18} />} label="Exames pendentes" value={dashboard?.pendingExams ?? 0} />
          <Metric icon={<ClipboardList size={18} />} label="Prescricoes" value={dashboard?.activePrescriptions ?? 0} />
          <Metric icon={<ReceiptText size={18} />} label="Faturas" value={dashboard?.invoices ?? 0} />
        </section>

        <section className="grid gap-4 lg:grid-cols-[1.1fr_0.9fr]">
          <div className="grid gap-4 md:grid-cols-2">
            <form className="rounded-md border border-slate-300 bg-white p-4 shadow-sm" onSubmit={createPatient}>
              <h2 className="mb-3 text-lg font-semibold">Paciente</h2>
              <Input label="Nome" value={patientForm.fullName} onChange={(value) => setPatientForm({ ...patientForm, fullName: value })} />
              <Input label="CPF" value={patientForm.cpf} onChange={(value) => setPatientForm({ ...patientForm, cpf: value })} />
              <Input label="Nascimento" type="date" value={patientForm.birthDate} onChange={(value) => setPatientForm({ ...patientForm, birthDate: value })} />
              <Input label="Convenio" value={patientForm.insuranceName} onChange={(value) => setPatientForm({ ...patientForm, insuranceName: value })} />
              <SubmitButton>Cadastrar</SubmitButton>
            </form>

            <form className="rounded-md border border-slate-300 bg-white p-4 shadow-sm" onSubmit={createAdmission}>
              <h2 className="mb-3 text-lg font-semibold">Internacao</h2>
              <Select label="Paciente" value={admissionPatientId} onChange={setAdmissionPatientId} options={patients.map((p) => ({ value: p.id, label: p.fullName }))} />
              <Select label="Leito" value={admissionBedId} onChange={setAdmissionBedId} options={beds.filter((b) => b.status === "DISPONIVEL").map((b) => ({ value: b.id, label: `${b.code} - ${b.ward}` }))} />
              <SubmitButton>Internar</SubmitButton>
            </form>

            <form className="rounded-md border border-slate-300 bg-white p-4 shadow-sm" onSubmit={createPrescription}>
              <h2 className="mb-3 text-lg font-semibold">Prescricao</h2>
              <Select label="Internacao" value={prescriptionForm.admissionId} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, admissionId: value })} options={activeAdmissions.map((a) => ({ value: a.id, label: `${a.patientName} / ${a.bedCode}` }))} />
              <Input label="Medicamento" value={prescriptionForm.medicineName} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, medicineName: value })} />
              <Input label="Dose" value={prescriptionForm.dose} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, dose: value })} />
              <Input label="Frequencia (h)" type="number" value={prescriptionForm.frequencyHours} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, frequencyHours: value })} />
              <Input label="Duracao (h)" type="number" value={prescriptionForm.durationHours} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, durationHours: value })} />
              <SubmitButton>Prescrever</SubmitButton>
            </form>

            <form className="rounded-md border border-slate-300 bg-white p-4 shadow-sm" onSubmit={createExam}>
              <h2 className="mb-3 text-lg font-semibold">Exame</h2>
              <Select label="Internacao" value={examAdmissionId} onChange={setExamAdmissionId} options={activeAdmissions.map((a) => ({ value: a.id, label: `${a.patientName} / ${a.bedCode}` }))} />
              <Input label="Tipo" value={examType} onChange={setExamType} />
              <SubmitButton>Solicitar</SubmitButton>
            </form>
          </div>

          <div className="rounded-md border border-slate-300 bg-white p-4 shadow-sm">
            <div className="mb-3 flex items-center gap-2">
              <AlertTriangle size={18} className="text-amber-700" />
              <h2 className="text-lg font-semibold">Alertas</h2>
            </div>
            <div className="space-y-2">
              {dashboard?.alerts.length ? (
                dashboard.alerts.map((alert, index) => (
                  <div key={`${alert.source}-${index}`} className="rounded-md border border-slate-200 bg-slate-50 px-3 py-2">
                    <div className="text-sm font-semibold">{alert.title}</div>
                    <div className="text-sm text-slate-600">{alert.message}</div>
                  </div>
                ))
              ) : (
                <p className="text-sm text-slate-500">{loading ? "Carregando..." : "Sem alertas."}</p>
              )}
            </div>
          </div>
        </section>

        <section className="grid gap-4 lg:grid-cols-2">
          <Table title="Internacoes" rows={admissions.map((a) => [a.patientName, a.bedCode, a.status, <button key={a.id} className="text-sm font-semibold text-red-700" onClick={() => void patch(`/admissions/${a.id}/discharge`, "Alta processada e fatura gerada.")}>Alta</button>])} />
          <Table title="Prescricoes" rows={prescriptions.map((p) => [p.medicineName, p.dose, p.status, <button key={p.id} className="text-sm font-semibold text-blue-700" onClick={() => void patch(`/prescriptions/${p.id}/complete`, "Prescricao finalizada.")}>Finalizar</button>])} />
          <Table title="Exames" rows={exams.map((e) => [e.examType, e.status, new Date(e.requestedAt).toLocaleString("pt-BR"), <button key={e.id} className="text-sm font-semibold text-blue-700" onClick={() => void patch(`/exams/${e.id}/complete`, "Exame concluido.")}>Concluir</button>])} />
          <Table title="Faturamento" rows={invoices.map((i) => [i.status, i.amount.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }), new Date(i.createdAt).toLocaleString("pt-BR")])} />
        </section>
      </div>
    </main>
  );
}

function Metric({ icon, label, value }: { icon: React.ReactNode; label: string; value: number }) {
  return (
    <div className="rounded-md border border-slate-300 bg-white p-4 shadow-sm">
      <div className="mb-2 text-slate-600">{icon}</div>
      <div className="text-2xl font-semibold">{value}</div>
      <div className="text-sm text-slate-600">{label}</div>
    </div>
  );
}

function Input({ label, value, onChange, type = "text" }: { label: string; value: string; type?: string; onChange: (value: string) => void }) {
  return (
    <label className="mb-3 block text-sm font-medium text-slate-700">
      {label}
      <input
        className="mt-1 h-10 w-full rounded-md border border-slate-300 px-3 text-sm outline-none focus:border-slate-950"
        type={type}
        value={value}
        onChange={(event) => onChange(event.target.value)}
      />
    </label>
  );
}

function Select({ label, value, onChange, options }: { label: string; value: string; onChange: (value: string) => void; options: { value: string; label: string }[] }) {
  return (
    <label className="mb-3 block text-sm font-medium text-slate-700">
      {label}
      <select
        className="mt-1 h-10 w-full rounded-md border border-slate-300 px-3 text-sm outline-none focus:border-slate-950"
        value={value}
        onChange={(event) => onChange(event.target.value)}
      >
        <option value="">Selecione</option>
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </label>
  );
}

function SubmitButton({ children }: { children: React.ReactNode }) {
  return (
    <button className="mt-1 h-10 w-full rounded-md bg-slate-950 px-4 text-sm font-semibold text-white hover:bg-slate-800" type="submit">
      {children}
    </button>
  );
}

function Table({ title, rows }: { title: string; rows: React.ReactNode[][] }) {
  return (
    <div className="overflow-hidden rounded-md border border-slate-300 bg-white shadow-sm">
      <div className="border-b border-slate-200 px-4 py-3 text-lg font-semibold">{title}</div>
      <div className="overflow-x-auto">
        <table className="w-full min-w-[480px] text-left text-sm">
          <tbody>
            {rows.length ? (
              rows.map((row, index) => (
                <tr key={index} className="border-b border-slate-100 last:border-0">
                  {row.map((cell, cellIndex) => (
                    <td key={cellIndex} className="px-4 py-3 align-top text-slate-700">
                      {cell}
                    </td>
                  ))}
                </tr>
              ))
            ) : (
              <tr>
                <td className="px-4 py-4 text-slate-500">Sem registros.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
