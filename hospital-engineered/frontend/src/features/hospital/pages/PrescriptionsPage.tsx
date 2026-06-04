import { StatusBadge } from "@/shared/components/StatusBadge";
import { DataTable } from "../components/DataTable";
import { ActionButton, FormPanel, Input, Select, SubmitButton } from "../components/FormControls";
import type { HospitalData } from "../types";

export function PrescriptionsPage({ hospital }: { hospital: HospitalData }) {
  const { activeAdmissions, completePrescription, createPrescription, prescriptionForm, prescriptions, setPrescriptionForm } = hospital;

  return (
    <section className="grid gap-5 lg:grid-cols-[340px_1fr]">
      <FormPanel title="Nova prescricao">
        <form onSubmit={createPrescription}>
          <Select label="Internacao" value={prescriptionForm.admissionId} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, admissionId: value })} options={activeAdmissions.map((admission) => ({ value: admission.id, label: `${admission.patientName} / ${admission.bedCode}` }))} />
          <Input label="Medicamento" value={prescriptionForm.medicineName} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, medicineName: value })} />
          <Input label="Dose" value={prescriptionForm.dose} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, dose: value })} />
          <Input label="Frequencia (h)" type="number" value={prescriptionForm.frequencyHours} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, frequencyHours: value })} />
          <Input label="Duracao (h)" type="number" value={prescriptionForm.durationHours} onChange={(value) => setPrescriptionForm({ ...prescriptionForm, durationHours: value })} />
          <SubmitButton>Prescrever</SubmitButton>
        </form>
      </FormPanel>
      <DataTable
        title="Prescricoes"
        rows={prescriptions.map((prescription) => [
          prescription.medicineName,
          prescription.dose,
          <StatusBadge key="status" status={prescription.status} />,
          prescription.status === "ATIVA" ? (
            <ActionButton key="action" onClick={() => void completePrescription(prescription.id)}>
              Finalizar
            </ActionButton>
          ) : (
            <span key="done" className="text-sm text-slate-500">
              Finalizada
            </span>
          ),
        ])}
      />
    </section>
  );
}
