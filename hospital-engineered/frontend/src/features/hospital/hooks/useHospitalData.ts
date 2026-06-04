"use client";

import { FormEvent, useEffect, useMemo, useState } from "react";
import { hospitalApi } from "@/shared/api/hospital-api";
import type { AdmissionDto, BedDto, DashboardDto, ExamRequestDto, InvoiceDto, PatientDto, PrescriptionDto } from "@/shared/types/hospital";
import type { HospitalData, PatientForm, PrescriptionForm } from "../types";

const initialPatientForm: PatientForm = {
  fullName: "Joao Arquitetura",
  cpf: "98765432100",
  birthDate: "1994-05-10",
  insuranceName: "Plano Aula",
};

const initialPrescriptionForm: PrescriptionForm = {
  admissionId: "",
  medicineName: "Cefalexina",
  dose: "500mg",
  frequencyHours: "8",
  durationHours: "24",
};

export function useHospitalData(): HospitalData {
  const [dashboard, setDashboard] = useState<DashboardDto | null>(null);
  const [patients, setPatients] = useState<PatientDto[]>([]);
  const [beds, setBeds] = useState<BedDto[]>([]);
  const [admissions, setAdmissions] = useState<AdmissionDto[]>([]);
  const [prescriptions, setPrescriptions] = useState<PrescriptionDto[]>([]);
  const [exams, setExams] = useState<ExamRequestDto[]>([]);
  const [invoices, setInvoices] = useState<InvoiceDto[]>([]);
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const [patientForm, setPatientForm] = useState<PatientForm>(initialPatientForm);
  const [admissionPatientId, setAdmissionPatientId] = useState("");
  const [admissionBedId, setAdmissionBedId] = useState("");
  const [prescriptionForm, setPrescriptionForm] = useState<PrescriptionForm>(initialPrescriptionForm);
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

  async function run(successMessage: string, action: () => Promise<unknown>) {
    try {
      await action();
      setMessage(successMessage);
      await refresh();
    } catch (error) {
      setMessage(error instanceof Error ? error.message : "Operacao recusada.");
    }
  }

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

  async function dischargeAdmission(admissionId: string) {
    await run("Alta processada e evento enviado ao BillingService.", () => hospitalApi.discharge(admissionId));
  }

  async function completePrescription(prescriptionId: string) {
    await run("Prescricao finalizada.", () => hospitalApi.completePrescription(prescriptionId));
  }

  async function completeExam(examId: string) {
    await run("Exame concluido.", () => hospitalApi.completeExam(examId));
  }

  return {
    dashboard,
    patients,
    beds,
    admissions,
    prescriptions,
    exams,
    invoices,
    message,
    loading,
    patientForm,
    admissionPatientId,
    admissionBedId,
    prescriptionForm,
    examAdmissionId,
    examType,
    activeAdmissions,
    availableBeds,
    setPatientForm,
    setAdmissionPatientId,
    setAdmissionBedId,
    setPrescriptionForm,
    setExamAdmissionId,
    setExamType,
    refresh,
    createPatient,
    createAdmission,
    createPrescription,
    createExam,
    dischargeAdmission,
    completePrescription,
    completeExam,
  };
}
