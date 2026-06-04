import { DataTable } from "../components/DataTable";
import { FormPanel, Input, SubmitButton } from "../components/FormControls";
import type { HospitalData } from "../types";

export function PatientsPage({ hospital }: { hospital: HospitalData }) {
  const { createPatient, patientForm, patients, setPatientForm } = hospital;

  return (
    <section className="grid gap-5 lg:grid-cols-[340px_1fr]">
      <FormPanel title="Novo paciente">
        <form onSubmit={createPatient}>
          <Input label="Nome" value={patientForm.fullName} onChange={(value) => setPatientForm({ ...patientForm, fullName: value })} />
          <Input label="CPF" value={patientForm.cpf} onChange={(value) => setPatientForm({ ...patientForm, cpf: value })} />
          <Input label="Nascimento" type="date" value={patientForm.birthDate} onChange={(value) => setPatientForm({ ...patientForm, birthDate: value })} />
          <Input label="Convenio" value={patientForm.insuranceName} onChange={(value) => setPatientForm({ ...patientForm, insuranceName: value })} />
          <SubmitButton>Cadastrar</SubmitButton>
        </form>
      </FormPanel>
      <DataTable
        title="Pacientes cadastrados"
        rows={patients.map((patient) => [patient.fullName, patient.cpf, new Date(patient.birthDate).toLocaleDateString("pt-BR"), patient.insuranceName])}
      />
    </section>
  );
}
