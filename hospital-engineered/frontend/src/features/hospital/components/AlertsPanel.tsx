import { AlertTriangle } from "lucide-react";
import type { DashboardDto } from "@/shared/types/hospital";

export function AlertsPanel({ alerts, loading }: { alerts: DashboardDto["alerts"]; loading: boolean }) {
  return (
    <div className="rounded-md border border-slate-200 bg-white p-4">
      <div className="mb-3 flex items-center gap-2">
        <AlertTriangle size={18} className="text-amber-700" />
        <h2 className="text-lg font-semibold">Alertas</h2>
      </div>
      <div className="space-y-2">
        {alerts.length ? (
          alerts.map((alert, index) => (
            <div key={`${alert.source}-${index}`} className="rounded-md border border-slate-200 bg-slate-50 px-3 py-2">
              <div className="flex items-center justify-between gap-2">
                <strong className="text-sm">{alert.title}</strong>
                <span className="rounded bg-white px-2 py-1 text-xs font-semibold text-slate-600">{alert.source}</span>
              </div>
              <p className="mt-1 text-sm text-slate-600">{alert.message}</p>
            </div>
          ))
        ) : (
          <p className="text-sm text-slate-500">{loading ? "Carregando..." : "Sem alertas."}</p>
        )}
      </div>
    </div>
  );
}
