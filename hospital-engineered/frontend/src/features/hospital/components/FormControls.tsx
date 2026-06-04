import type { ReactNode } from "react";

export function FormPanel({ title, children }: { title: string; children: ReactNode }) {
  return (
    <div className="rounded-md border border-slate-200 bg-white p-4">
      <h2 className="mb-3 text-lg font-semibold">{title}</h2>
      {children}
    </div>
  );
}

export function Input({ label, value, onChange, type = "text" }: { label: string; value: string; type?: string; onChange: (value: string) => void }) {
  return (
    <label className="mb-3 block text-sm font-medium text-slate-700">
      {label}
      <input
        className="mt-1 h-10 w-full rounded-md border border-slate-300 px-3 text-sm outline-none focus:border-teal-700"
        type={type}
        value={value}
        onChange={(event) => onChange(event.target.value)}
      />
    </label>
  );
}

export function Select({ label, value, onChange, options }: { label: string; value: string; onChange: (value: string) => void; options: { value: string; label: string }[] }) {
  return (
    <label className="mb-3 block text-sm font-medium text-slate-700">
      {label}
      <select className="mt-1 h-10 w-full rounded-md border border-slate-300 px-3 text-sm outline-none focus:border-teal-700" value={value} onChange={(event) => onChange(event.target.value)}>
        <option value="">Selecione</option>
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </label>
  );
}

export function SubmitButton({ children }: { children: ReactNode }) {
  return (
    <button className="mt-1 h-10 w-full rounded-md bg-teal-700 px-4 text-sm font-semibold text-white hover:bg-teal-800" type="submit">
      {children}
    </button>
  );
}

export function ActionButton({ children, onClick }: { children: ReactNode; onClick: () => void }) {
  return (
    <button className="text-sm font-semibold text-blue-700 hover:text-blue-900" onClick={onClick} type="button">
      {children}
    </button>
  );
}
