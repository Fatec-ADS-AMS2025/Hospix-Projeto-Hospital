import type { FormEvent } from "react";
import type {
  AdmissionDto,
  BedDto,
  DashboardDto,
  ExamRequestDto,
  InvoiceDto,
  PatientDto,
  PrescriptionDto,
} from "@/shared/types/hospital";

export type HospitalSection = "overview" | "patients" | "admissions" | "prescriptions" | "exams";

export type PatientForm = {
  fullName: string;
  cpf: string;
  birthDate: string;
  insuranceName: string;
};

export type PrescriptionForm = {
  admissionId: string;
  medicineName: string;
  dose: string;
  frequencyHours: string;
  durationHours: string;
};

export type HospitalData = {
  dashboard: DashboardDto | null;
  patients: PatientDto[];
  beds: BedDto[];
  admissions: AdmissionDto[];
  prescriptions: PrescriptionDto[];
  exams: ExamRequestDto[];
  invoices: InvoiceDto[];
  message: string;
  loading: boolean;
  patientForm: PatientForm;
  admissionPatientId: string;
  admissionBedId: string;
  prescriptionForm: PrescriptionForm;
  examAdmissionId: string;
  examType: string;
  activeAdmissions: AdmissionDto[];
  availableBeds: BedDto[];
  setPatientForm: (form: PatientForm) => void;
  setAdmissionPatientId: (value: string) => void;
  setAdmissionBedId: (value: string) => void;
  setPrescriptionForm: (form: PrescriptionForm) => void;
  setExamAdmissionId: (value: string) => void;
  setExamType: (value: string) => void;
  refresh: () => Promise<void>;
  createPatient: (event: FormEvent<HTMLFormElement>) => Promise<void>;
  createAdmission: (event: FormEvent<HTMLFormElement>) => Promise<void>;
  createPrescription: (event: FormEvent<HTMLFormElement>) => Promise<void>;
  createExam: (event: FormEvent<HTMLFormElement>) => Promise<void>;
  dischargeAdmission: (admissionId: string) => Promise<void>;
  completePrescription: (prescriptionId: string) => Promise<void>;
  completeExam: (examId: string) => Promise<void>;
};
