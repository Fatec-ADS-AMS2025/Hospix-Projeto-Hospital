"use client";

import { RefreshCw, ShieldCheck } from "lucide-react";
import Link from "next/link";
import { ReactNode, useSyncExternalStore } from "react";
import type { HospitalSection } from "../types";

const profiles = ["Medico", "Enfermagem", "Administracao"] as const;
type HospitalProfile = (typeof profiles)[number];
const profileStorageKey = "hospital-engineered-profile";
const profileChangeEvent = "hospital-engineered-profile-change";

const navItems: { section: HospitalSection; href: string; label: string }[] = [
  { section: "overview", href: "/", label: "Visao geral" },
  { section: "patients", href: "/patients", label: "Pacientes" },
  { section: "admissions", href: "/admissions", label: "Internacoes" },
  { section: "prescriptions", href: "/prescriptions", label: "Prescricoes" },
  { section: "exams", href: "/exams", label: "Exames/Faturamento" },
];

const sectionTitles: Record<HospitalSection, string> = {
  overview: "Visao geral",
  patients: "Pacientes",
  admissions: "Internacoes",
  prescriptions: "Prescricoes",
  exams: "Exames e faturamento",
};

type HospitalShellProps = {
  children: ReactNode;
  message: string;
  section: HospitalSection;
  onRefresh: () => Promise<void>;
};

function isHospitalProfile(value: string | null): value is HospitalProfile {
  return profiles.some((item) => item === value);
}

function getStoredProfile(): HospitalProfile {
  if (typeof window === "undefined") {
    return "Medico";
  }

  const savedProfile = window.localStorage.getItem(profileStorageKey);
  return isHospitalProfile(savedProfile) ? savedProfile : "Medico";
}

function subscribeToProfile(callback: () => void) {
  window.addEventListener("storage", callback);
  window.addEventListener(profileChangeEvent, callback);

  return () => {
    window.removeEventListener("storage", callback);
    window.removeEventListener(profileChangeEvent, callback);
  };
}

export function HospitalShell({ children, message, section, onRefresh }: HospitalShellProps) {
  const profile = useSyncExternalStore(subscribeToProfile, getStoredProfile, () => "Medico");

  function selectProfile(nextProfile: HospitalProfile) {
    window.localStorage.setItem(profileStorageKey, nextProfile);
    window.dispatchEvent(new Event(profileChangeEvent));
  }

  return (
    <main className="min-h-screen bg-[#f7faf9] text-slate-950">
      <div className="mx-auto flex max-w-7xl flex-col gap-6 px-4 py-5 sm:px-6 lg:px-8">
        <header className="flex flex-col gap-4 border-b border-slate-200 pb-5">
          <div className="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
            <div>
              <p className="text-sm font-semibold uppercase tracking-[0.18em] text-teal-700">hospital-engineered</p>
              <h1 className="text-3xl font-semibold tracking-normal">Sistema Hospitalar Inteligente</h1>
              <p className="mt-1 text-sm text-slate-600">{sectionTitles[section]}</p>
            </div>
            <div className="flex flex-col gap-3 sm:flex-row sm:items-center">
              <div className="inline-flex rounded-md border border-slate-300 bg-white p-1">
                {profiles.map((item) => (
                  <button
                    key={item}
                    className={`h-9 rounded px-3 text-sm font-semibold ${profile === item ? "bg-slate-950 text-white" : "text-slate-700 hover:bg-slate-100"}`}
                    onClick={() => selectProfile(item)}
                    type="button"
                  >
                    {item}
                  </button>
                ))}
              </div>
              <button
                className="inline-flex h-10 items-center justify-center gap-2 rounded-md bg-teal-700 px-4 text-sm font-semibold text-white hover:bg-teal-800"
                onClick={() => void onRefresh()}
                type="button"
              >
                <RefreshCw size={16} />
                Atualizar
              </button>
            </div>
          </div>

          <nav className="flex gap-2 overflow-x-auto pb-1">
            {navItems.map((item) => (
              <Link
                key={item.section}
                className={`whitespace-nowrap rounded-md border px-3 py-2 text-sm font-semibold ${
                  section === item.section ? "border-teal-700 bg-teal-700 text-white" : "border-slate-200 bg-white text-slate-700 hover:border-teal-300"
                }`}
                href={item.href}
              >
                {item.label}
              </Link>
            ))}
          </nav>
        </header>

        {message ? <div className="rounded-md border border-blue-200 bg-blue-50 px-4 py-3 text-sm text-blue-950">{message}</div> : null}

        {children}

        <footer className="flex items-center gap-2 border-t border-slate-200 py-4 text-sm text-slate-600">
          <ShieldCheck size={16} className="text-teal-700" />
          Perfil ativo: {profile}
        </footer>
      </div>
    </main>
  );
}
