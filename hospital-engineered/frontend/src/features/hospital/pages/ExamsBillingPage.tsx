import { StatusBadge } from "@/shared/components/StatusBadge";
import { DataTable } from "../components/DataTable";
import { ActionButton, FormPanel, Input, Select, SubmitButton } from "../components/FormControls";
import type { HospitalData } from "../types";

export function ExamsBillingPage({ hospital }: { hospital: HospitalData }) {
  const { activeAdmissions, completeExam, createExam, examAdmissionId, examType, exams, invoices, setExamAdmissionId, setExamType } = hospital;

  return (
    <section className="grid gap-5 lg:grid-cols-[340px_1fr]">
      <FormPanel title="Novo exame">
        <form onSubmit={createExam}>
          <Select label="Internacao" value={examAdmissionId} onChange={setExamAdmissionId} options={activeAdmissions.map((admission) => ({ value: admission.id, label: `${admission.patientName} / ${admission.bedCode}` }))} />
          <Input label="Tipo" value={examType} onChange={setExamType} />
          <SubmitButton>Solicitar</SubmitButton>
        </form>
      </FormPanel>
      <div className="grid gap-4">
        <DataTable
          title="Exames"
          rows={exams.map((exam) => [
            exam.examType,
            <StatusBadge key="status" status={exam.status} />,
            new Date(exam.requestedAt).toLocaleString("pt-BR"),
            exam.status === "PENDENTE" ? (
              <ActionButton key="action" onClick={() => void completeExam(exam.id)}>
                Concluir
              </ActionButton>
            ) : (
              <span key="done" className="text-sm text-slate-500">
                Concluido
              </span>
            ),
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
      </div>
    </section>
  );
}
