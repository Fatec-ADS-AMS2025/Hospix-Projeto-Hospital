type StatusBadgeProps = {
  status: string;
};

export function StatusBadge({ status }: StatusBadgeProps) {
  const normalized = status.toUpperCase();
  const className =
    normalized === "INTERNADO" || normalized === "ATIVA" || normalized === "PENDENTE"
      ? "border-amber-300 bg-amber-50 text-amber-900"
      : normalized === "DISPONIVEL" || normalized === "CONCLUIDO" || normalized === "FINALIZADA" || normalized === "ALTA"
        ? "border-teal-300 bg-teal-50 text-teal-900"
        : "border-slate-300 bg-slate-50 text-slate-800";

  return <span className={`inline-flex rounded-md border px-2 py-1 text-xs font-semibold ${className}`}>{status}</span>;
}
