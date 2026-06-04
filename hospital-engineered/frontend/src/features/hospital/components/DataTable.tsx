import type { ReactNode } from "react";

export function DataTable({ title, rows }: { title: string; rows: ReactNode[][] }) {
  return (
    <div className="overflow-hidden rounded-md border border-slate-200 bg-white">
      <div className="border-b border-slate-200 px-4 py-3 text-lg font-semibold">{title}</div>
      <div className="overflow-x-auto">
        <table className="w-full min-w-[500px] text-left text-sm">
          <tbody>
            {rows.length ? (
              rows.map((row, index) => (
                <tr key={index} className="border-b border-slate-100 last:border-0">
                  {row.map((cell, cellIndex) => (
                    <td key={cellIndex} className="px-4 py-3 align-top text-slate-700">
                      {cell}
                    </td>
                  ))}
                </tr>
              ))
            ) : (
              <tr>
                <td className="px-4 py-4 text-slate-500">Sem registros.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
