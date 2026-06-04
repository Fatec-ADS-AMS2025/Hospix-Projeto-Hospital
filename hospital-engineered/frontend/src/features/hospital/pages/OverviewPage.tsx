import { BedDouble, ClipboardCheck, FlaskConical, ReceiptText, Stethoscope, UserPlus } from "lucide-react";
import { MetricCard } from "@/shared/components/MetricCard";
import { StatusBadge } from "@/shared/components/StatusBadge";
import type { DashboardDto } from "@/shared/types/hospital";
import { AlertsPanel } from "../components/AlertsPanel";
import { DataTable } from "../components/DataTable";

export function OverviewPage({ dashboard, loading }: { dashboard: DashboardDto | null; loading: boolean }) {
  return (
    <div className="grid gap-5">
      <section className="grid gap-3 sm:grid-cols-2 lg:grid-cols-6">
        <MetricCard icon={<UserPlus size={18} />} label="Pacientes" value={dashboard?.patients ?? 0} tone="teal" />
        <MetricCard icon={<Stethoscope size={18} />} label="Internados" value={dashboard?.activeAdmissions ?? 0} tone="blue" />
        <MetricCard icon={<BedDouble size={18} />} label="Leitos ocupados" value={dashboard?.occupiedBeds ?? 0} tone="amber" />
        <MetricCard icon={<FlaskConical size={18} />} label="Exames pendentes" value={dashboard?.pendingExams ?? 0} tone="rose" />
        <MetricCard icon={<ClipboardCheck size={18} />} label="Prescricoes" value={dashboard?.activePrescriptions ?? 0} tone="slate" />
        <MetricCard icon={<ReceiptText size={18} />} label="Faturas" value={dashboard?.invoices ?? 0} tone="teal" />
      </section>

      <section className="grid gap-4 lg:grid-cols-[1fr_420px]">
        <DataTable
          title="Internacoes recentes"
          rows={(dashboard?.admissions ?? []).map((admission) => [
            admission.patientName,
            admission.bedCode,
            <StatusBadge key="status" status={admission.status} />,
            new Date(admission.admittedAt).toLocaleString("pt-BR"),
          ])}
        />
        <AlertsPanel alerts={dashboard?.alerts ?? []} loading={loading} />
      </section>
    </div>
  );
}
