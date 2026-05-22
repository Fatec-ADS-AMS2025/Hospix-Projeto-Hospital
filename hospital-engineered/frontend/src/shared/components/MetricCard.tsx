import type { ReactNode } from "react";

type MetricCardProps = {
  icon: ReactNode;
  label: string;
  value: number;
  tone: "teal" | "blue" | "amber" | "rose" | "slate";
};

const toneClass: Record<MetricCardProps["tone"], string> = {
  teal: "border-teal-200 bg-teal-50 text-teal-900",
  blue: "border-blue-200 bg-blue-50 text-blue-950",
  amber: "border-amber-200 bg-amber-50 text-amber-950",
  rose: "border-rose-200 bg-rose-50 text-rose-950",
  slate: "border-slate-200 bg-slate-50 text-slate-950",
};

export function MetricCard({ icon, label, value, tone }: MetricCardProps) {
  return (
    <div className={`rounded-md border p-4 ${toneClass[tone]}`}>
      <div className="mb-3 flex items-center justify-between">
        <span className="text-current">{icon}</span>
        <span className="text-2xl font-semibold">{value}</span>
      </div>
      <p className="text-sm font-medium">{label}</p>
    </div>
  );
}
