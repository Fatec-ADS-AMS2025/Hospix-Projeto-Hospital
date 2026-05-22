export type PatientDto = {
  id: string;
  fullName: string;
  cpf: string;
  birthDate: string;
  insuranceName: string;
};

export type BedDto = {
  id: string;
  code: string;
  ward: string;
  status: string;
};

export type AdmissionDto = {
  id: string;
  patientId: string;
  patientName: string;
  bedCode: string;
  status: string;
  admittedAt: string;
  dischargedAt: string | null;
};

export type PrescriptionDto = {
  id: string;
  admissionId: string;
  medicineName: string;
  dose: string;
  frequencyHours: number;
  startAt: string;
  endAt: string;
  status: string;
};

export type ExamRequestDto = {
  id: string;
  admissionId: string;
  examType: string;
  status: string;
  requestedAt: string;
  completedAt: string | null;
};

export type InvoiceDto = {
  id: string;
  admissionId: string;
  amount: number;
  status: string;
  createdAt: string;
};

export type AlertDto = {
  level: string;
  title: string;
  message: string;
  source: string;
};

export type DashboardDto = {
  patients: number;
  activeAdmissions: number;
  occupiedBeds: number;
  pendingExams: number;
  activePrescriptions: number;
  invoices: number;
  admissions: AdmissionDto[];
  alerts: AlertDto[];
};

export type CreatePatientRequest = {
  fullName: string;
  cpf: string;
  birthDate: string;
  insuranceName: string;
};

export type CreateAdmissionRequest = {
  patientId: string;
  bedId: string;
};

export type CreatePrescriptionRequest = {
  admissionId: string;
  medicineName: string;
  dose: string;
  frequencyHours: number;
  startAt: string;
  endAt: string;
};

export type CreateExamRequest = {
  admissionId: string;
  examType: string;
};
