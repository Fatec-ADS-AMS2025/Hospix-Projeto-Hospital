import type {
  AdmissionDto,
  AlertDto,
  BedDto,
  CreateAdmissionRequest,
  CreateExamRequest,
  CreatePatientRequest,
  CreatePrescriptionRequest,
  DashboardDto,
  ExamRequestDto,
  InvoiceDto,
  PatientDto,
  PrescriptionDto,
} from "@/shared/types/hospital";

const API = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5101/api";

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
    throw new Error(body || "Falha na API.");
  }

  return response.json() as Promise<T>;
}

export const hospitalApi = {
  dashboard: () => request<DashboardDto>("/dashboard"),
  patients: () => request<PatientDto[]>("/patients"),
  createPatient: (body: CreatePatientRequest) => request<PatientDto>("/patients", { method: "POST", body: JSON.stringify(body) }),
  beds: () => request<BedDto[]>("/beds"),
  admissions: () => request<AdmissionDto[]>("/admissions"),
  createAdmission: (body: CreateAdmissionRequest) => request<AdmissionDto>("/admissions", { method: "POST", body: JSON.stringify(body) }),
  discharge: (id: string) => request<AdmissionDto>(`/admissions/${id}/discharge`, { method: "PATCH" }),
  prescriptions: () => request<PrescriptionDto[]>("/prescriptions"),
  createPrescription: (body: CreatePrescriptionRequest) => request<PrescriptionDto>("/prescriptions", { method: "POST", body: JSON.stringify(body) }),
  completePrescription: (id: string) => request<PrescriptionDto>(`/prescriptions/${id}/complete`, { method: "PATCH" }),
  exams: () => request<ExamRequestDto[]>("/exams"),
  createExam: (body: CreateExamRequest) => request<ExamRequestDto>("/exams", { method: "POST", body: JSON.stringify(body) }),
  completeExam: (id: string) => request<ExamRequestDto>(`/exams/${id}/complete`, { method: "PATCH" }),
  alerts: () => request<AlertDto[]>("/alerts"),
  invoices: () => request<InvoiceDto[]>("/invoices"),
};
