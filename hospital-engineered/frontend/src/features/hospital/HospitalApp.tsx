"use client";

import { HospitalShell } from "./components/HospitalShell";
import { useHospitalData } from "./hooks/useHospitalData";
import { AdmissionsPage } from "./pages/AdmissionsPage";
import { ExamsBillingPage } from "./pages/ExamsBillingPage";
import { OverviewPage } from "./pages/OverviewPage";
import { PatientsPage } from "./pages/PatientsPage";
import { PrescriptionsPage } from "./pages/PrescriptionsPage";
import type { HospitalSection } from "./types";

export function HospitalApp({ section = "overview" }: { section?: HospitalSection }) {
  const hospital = useHospitalData();

  return (
    <HospitalShell message={hospital.message} onRefresh={hospital.refresh} section={section}>
      {section === "overview" ? (
        <OverviewPage dashboard={hospital.dashboard} loading={hospital.loading} />
      ) : section === "patients" ? (
        <PatientsPage hospital={hospital} />
      ) : section === "admissions" ? (
        <AdmissionsPage hospital={hospital} />
      ) : section === "prescriptions" ? (
        <PrescriptionsPage hospital={hospital} />
      ) : (
        <ExamsBillingPage hospital={hospital} />
      )}
    </HospitalShell>
  );
}
