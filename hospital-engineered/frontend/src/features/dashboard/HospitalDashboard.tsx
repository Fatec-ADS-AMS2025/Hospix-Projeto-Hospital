"use client";

import {
  AlertTriangle,
  BedDouble,
  ClipboardCheck,
  FlaskConical,
  ReceiptText,
  RefreshCw,
  ShieldCheck,
  Stethoscope,
  UserPlus,
} from "lucide-react";
import { FormEvent, ReactNode, useEffect, useMemo, useState } from "react";
import { hospitalApi } from "@/shared/api/hospital-api";
import { MetricCard } from "@/shared/components/MetricCard";
import { StatusBadge } from "@/shared/components/StatusBadge";
import type {
  AdmissionDto,
  BedDto,
  DashboardDto,
  ExamRequestDto,
  InvoiceDto,
  PatientDto,
  PrescriptionDto,
} from "@/shared/types/hospital";

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

const profiles = ["Medico", "Enfermagem", "Administracao"] as const;

export function HospitalDashboard() {
  const [profile, setProfile] = useState<(typeof profiles)[number]>("Medico");
  const [dashboard, setDashboard] = useState<DashboardDto | null>(null);
  const [patients, setPatients] = useState<PatientDto[]>([]);
  const [beds, setBeds] = useState<BedDto[]>([]);
  const [admissions, setAdmissions] = useState<AdmissionDto[]>([]);
  const [prescriptions, setPrescriptions] = useState<PrescriptionDto[]>([]);
  const [exams, setExams] = useState<ExamRequestDto[]>([]);
  const [invoices, setInvoices] = useState<InvoiceDto[]>([]);
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);

  const [patientForm, setPatientForm] = useState<PatientForm>({
    fullName: "Joao Arquitetura",
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

  const activeAdmissions = useMemo(() => admissions.filter((admission) => admission.status === "INTERNADO"), [admissions]);
  const availableBeds = useMemo(() => beds.filter((bed) => bed.status === "DISPONIVEL"), [beds]);

  async function refresh() {
    setLoading(true);
    setMessage("");
    try {
      const [nextDashboard, nextPatients, nextBeds, nextAdmissions, nextPrescriptions, nextExams, nextInvoices] = await Promise.all([
        hospitalApi.dashboard(),
        hospitalApi.patients(),
        hospitalApi.beds(),
        hospitalApi.admissions(),
        hospitalApi.prescriptions(),
        hospitalApi.exams(),
        hospitalApi.invoices(),
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
      setPrescriptionForm((current) => ({ ...current, admissionId: nextAdmissions.find((admission) => admission.status === "INTERNADO")?.id ?? "" }));
      setExamAdmissionId(nextAdmissions.find((admission) => admission.status === "INTERNADO")?.id ?? "");
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "Falha ao carregar dados.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    const timer = window.setTimeout(() => {
      void refresh();
    }, 0);

    return () => window.clearTimeout(timer);
  }, []);

  async function createPatient(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    await run("Paciente cadastrado.", () => hospitalApi.createPatient(patientForm));
  }

  async function createAdmission(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    await run("Internacao criada.", () => hospitalApi.createAdmission({ patientId: admissionPatientId, bedId: admissionBedId }));
  }

  async function createPrescription(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const startAt = new Date();
    const endAt = new Date(startAt.getTime() + Number(prescriptionForm.durationHours) * 60 * 60 * 1000);
    await run("Prescricao criada por use case dedicado.", () =>
      hospitalApi.createPrescription({
        admissionId: prescriptionForm.admissionId,
        medicineName: prescriptionForm.medicineName,
        dose: prescriptionForm.dose,
        frequencyHours: Number(prescriptionForm.frequencyHours),
        startAt: startAt.toISOString(),
        endAt: endAt.toISOString(),
      }),
    );
  }

  async function createExam(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    await run("Exame solicitado.", () => hospitalApi.createExam({ admissionId: examAdmissionId, examType }));
  }

  async function run(successMessage: string, action: () => Promise<unknown>) {
    try {
      await action();
      setMessage(successMessage);
      await refresh();
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "Operacao recusada.");
    }
  }

  return (
    <main className="min-h-screen bg-[#f7faf9] text-slate-950">
      <div className="mx-auto flex max-w-7xl flex-col gap-6 px-4 py-5 sm:px-6 lg:px-8">
        <header className="flex flex-col gap-4 border-b border-slate-200 pb-5 lg:flex-row lg:items-center lg:justify-between">
          <div>
            <p className="text-sm font-semibold uppercase tracking-[0.18em] text-teal-700">hospital-engineered</p>
            <h1 className="text-3xl font-semibold tracking-normal">Sistema Hospitalar Inteligente</h1>
          </div>
          <div className="flex flex-col gap-3 sm:flex-row sm:items-center">
            <div className="inline-flex rounded-md border border-slate-300 bg-white p-1">
              {profiles.map((item) => (
                <button
                  key={item}
                  className={`h-9 rounded px-3 text-sm font-semibold ${profile === item ? "bg-slate-950 text-white" : "text-slate-700 hover:bg-slate-100"}`}
                  onClick={() => setProfile(item)}
                  type="button"
                >
                  {item}
                </button>
              ))}
            </div>
            <button
              className="inline-flex h-10 items-center justify-center gap-2 rounded-md bg-teal-700 px-4 text-sm font-semibold text-white hover:bg-teal-800"
              onClick={() => void refresh()}
              type="button"
            >
              <RefreshCw size={16} />
              Atualizar
            </button>
          </div>
        </header>

        {message ? <div className="rounded-md border border-blue-200 bg-blue-50 px-4 py-3 text-sm text-blue-950">{message}</div> : null}

        <section className="grid gap-3 sm:grid-cols-2 lg:grid-cols-6">
          <MetricCard icon={<UserPlus size={18} />} label="Pacientes" value={dashboard?.patients ?? 0} tone="teal" />
          <MetricCard icon={<Stethoscope size={18} />} label="Internados" value={dashboard?.activeAdmissions ?? 0} tone="blue" />
          <MetricCard icon={<BedDouble size={18} />} label="Leitos ocupados" value={dashboard?.occupiedBeds ?? 0} tone="amber" />
          <MetricCard icon={<FlaskConical size={18} />} label="Exames pendentes" value={dashboard?.pendingExams ?? 0} tone="rose" />
          <MetricCard icon={<ClipboardCheck size={18} />} label="Prescricoes" value={dashboard?.activePrescriptions ?? 0} tone="slate" />
          <MetricCard icon={<ReceiptText size={18} />} label="Faturas" value={dashboard?.invoices ?? 0} tone="teal" />
        </section>

        <section className="grid gap-5 lg:grid-cols-[320px_1fr]">
          <aside className="space-y-3">
            <FormPanel title="Paciente">
              <form onSubmit={createPatient}>
                <Input label="Nome" value={patientForm.fullName} onChange={(value) => setPatientForm({ ...patientForm, fullName: value })} />
                <Input label="CPF" value={patientForm.cpf} onChange={(value) => setPatientForm({ ...patientForm, cpf: value })} />
                <Input label="Nascimento" type="date" value={patientForm.birthDate} onChange={(value) => setPatientForm({ ...patientForm, birthDate: value })} />
                <Input label="Convenio" value={patientForm.insuranceName} onChange={(value) => setPatientForm({ ...patientForm, insuranceName: value })} />
                <SubmitButton>Cadastrar</SubmitButton>
              </form>
            </FormPanel>

            <FormPanel title="Internacao">
              <form onSubmit={createAdmission}>
                <Select label="Paciente" value={admissionPatientId} onChange={setAdmissionPatientId} options={patients.map((patient) => ({ value: patient.id, label: patient.fullName }))} />
                <Select label="Leito" value={admissionBedId} onChange={setAdmissionBedId} options={availableBeds.map((bed) => ({ value: bed.id, label: `${bed.code} - ${bed.ward}` }))} />
                <SubmitButton>Internar</SubmitButton>
              </form>
            </FormPanel>
          </aside>

          <div className="grid gap-5">
            <section className="grid gap-4 lg:grid-cols-[1fr_360px]">
              <div className="grid gap-3 md:grid-cols-2">
                <FormPanel title="Prescricao">
                  <form onSubmit={createPrescription}>
                    <Select label="Internacao" value={prescriptionForm.admissionId} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, admissionId: value })} options={activeAdmissions.map((admission) => ({ value: admission.id, label: `${admission.patientName} / ${admission.bedCode}` }))} />
                    <Input label="Medicamento" value={prescriptionForm.medicineName} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, medicineName: value })} />
                    <Input label="Dose" value={prescriptionForm.dose} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, dose: value })} />
                    <Input label="Frequencia (h)" type="number" value={prescriptionForm.frequencyHours} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, frequencyHours: value })} />
                    <Input label="Duracao (h)" type="number" value={prescriptionForm.durationHours} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, durationHours: value })} />
                    <SubmitButton>Prescrever</SubmitButton>
                  </form>
                </FormPanel>

                <FormPanel title="Exame">
                  <form onSubmit={createExam}>
                    <Select label="Internacao" value={examAdmissionId} onChange={setExamAdmissionId} options={activeAdmissions.map((admission) => ({ value: admission.id, label: `${admission.patientName} / ${admission.bedCode}` }))} />
                    <Input label="Tipo" value={examType} onChange={setExamType} />
                    <SubmitButton>Solicitar</SubmitButton>
                  </form>
                </FormPanel>
              </div>

              <div className="rounded-md border border-slate-200 bg-white p-4">
                <div className="mb-3 flex items-center gap-2">
                  <AlertTriangle size={18} className="text-amber-700" />
                  <h2 className="text-lg font-semibold">Alertas</h2>
                </div>
                <div className="space-y-2">
                  {dashboard?.alerts.length ? (
                    dashboard.alerts.map((alert, index) => (
                      <div key={`${alert.source}-${index}`} className="rounded-md border border-slate-200 bg-slate-50 px-3 py-2">
                        <div className="flex items-center justify-between gap-2">
                          <strong className="text-sm">{alert.title}</strong>
                          <span className="rounded bg-white px-2 py-1 text-xs font-semibold text-slate-600">{alert.source}</span>
                        </div>
                        <p className="mt-1 text-sm text-slate-600">{alert.message}</p>
                      </div>
                    ))
                  ) : (
                    <p className="text-sm text-slate-500">{loading ? "Carregando..." : "Sem alertas."}</p>
                  )}
                </div>
              </div>
            </section>

            <section className="grid gap-4 lg:grid-cols-2">
              <DataTable
                title="Internacoes"
                rows={admissions.map((admission) => [
                  admission.patientName,
                  admission.bedCode,
                  <StatusBadge key="status" status={admission.status} />,
                  <ActionButton key="action" onClick={() => run("Alta processada e evento enviado ao BillingService.", () => hospitalApi.discharge(admission.id))}>
                    Alta
                  </ActionButton>,
                ])}
              />
              <DataTable
                title="Prescricoes"
                rows={prescriptions.map((prescription) => [
                  prescription.medicineName,
                  prescription.dose,
                  <StatusBadge key="status" status={prescription.status} />,
                  <ActionButton key="action" onClick={() => run("Prescricao finalizada.", () => hospitalApi.completePrescription(prescription.id))}>
                    Finalizar
                  </ActionButton>,
                ])}
              />
              <DataTable
                title="Exames"
                rows={exams.map((exam) => [
                  exam.examType,
                  <StatusBadge key="status" status={exam.status} />,
                  new Date(exam.requestedAt).toLocaleString("pt-BR"),
                  <ActionButton key="action" onClick={() => run("Exame concluido.", () => hospitalApi.completeExam(exam.id))}>
                    Concluir
                  </ActionButton>,
                ])}
              />
              <DataTable
                title="Faturamento"
                rows={invoices.map((invoice) => [
                  <StatusBadge key="status" status={invoice.status} />,
                  invoice.amount.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }),
                  new Date(invoice.createdAt).toLocaleString("pt-BR"),
                ])}
              />
            </section>
          </div>
        </section>

        <footer className="flex items-center gap-2 border-t border-slate-200 py-4 text-sm text-slate-600">
          <ShieldCheck size={16} className="text-teal-700" />
          Perfil ativo: {profile}
        </footer>
      </div>
    </main>
  );
}

function FormPanel({ title, children }: { title: string; children: ReactNode }) {
  return (
    <div className="rounded-md border border-slate-200 bg-white p-4">
      <h2 className="mb-3 text-lg font-semibold">{title}</h2>
      {children}
    </div>
  );
}

function Input({ label, value, onChange, type = "text" }: { label: string; value: string; type?: string; onChange: (value: string) => void }) {
  return (
    <label className="mb-3 block text-sm font-medium text-slate-700">
      {label}
      <input
        className="mt-1 h-10 w-full rounded-md border border-slate-300 px-3 text-sm outline-none focus:border-teal-700"
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
      <select className="mt-1 h-10 w-full rounded-md border border-slate-300 px-3 text-sm outline-none focus:border-teal-700" value={value} onChange={(event) => onChange(event.target.value)}>
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

function SubmitButton({ children }: { children: ReactNode }) {
  return (
    <button className="mt-1 h-10 w-full rounded-md bg-teal-700 px-4 text-sm font-semibold text-white hover:bg-teal-800" type="submit">
      {children}
    </button>
  );
}

function ActionButton({ children, onClick }: { children: ReactNode; onClick: () => void }) {
  return (
    <button className="text-sm font-semibold text-blue-700 hover:text-blue-900" onClick={onClick} type="button">
      {children}
    </button>
  );
}

function DataTable({ title, rows }: { title: string; rows: ReactNode[][] }) {
  return (
    <div className="overflow-hidden rounded-md border border-slate-200 bg-white">
      <div className="border-b border-slate-200 px-4 py-3 text-lg font-semibold">{title}</div>
      <div className="overflow-x-auto">
        <table className="w-full min-w-[500px] text-left text-sm">
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
