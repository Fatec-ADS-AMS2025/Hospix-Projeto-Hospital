import { StatusBadge } from "@/shared/components/StatusBadge";
import { DataTable } from "../components/DataTable";
import { ActionButton, FormPanel, Select, SubmitButton } from "../components/FormControls";
import type { HospitalData } from "../types";

export function AdmissionsPage({ hospital }: { hospital: HospitalData }) {
  const {
    admissionBedId,
    admissionPatientId,
    admissions,
    availableBeds,
    createAdmission,
    dischargeAdmission,
    patients,
    setAdmissionBedId,
    setAdmissionPatientId,
  } = hospital;

  return (
    <section className="grid gap-5 lg:grid-cols-[340px_1fr]">
      <FormPanel title="Nova internacao">
        <form onSubmit={createAdmission}>
          <Select label="Paciente" value={admissionPatientId} onChange={setAdmissionPatientId} options={patients.map((patient) => ({ value: patient.id, label: patient.fullName }))} />
          <Select label="Leito" value={admissionBedId} onChange={setAdmissionBedId} options={availableBeds.map((bed) => ({ value: bed.id, label: `${bed.code} - ${bed.ward}` }))} />
          <SubmitButton>Internar</SubmitButton>
        </form>
      </FormPanel>
      <DataTable
        title="Internacoes"
        rows={admissions.map((admission) => [
          admission.patientName,
          admission.bedCode,
          <StatusBadge key="status" status={admission.status} />,
          admission.status === "INTERNADO" ? (
            <ActionButton key="action" onClick={() => void dischargeAdmission(admission.id)}>
              Alta
            </ActionButton>
          ) : (
            <span key="done" className="text-sm text-slate-500">
              Encerrada
            </span>
          ),
        ])}
      />
    </section>
  );
}
