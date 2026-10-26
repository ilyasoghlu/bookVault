"use client";

import { DataViewProps } from "./types";

export default function DataGrid<T>({
  data,
  config,
}: DataViewProps<T>) {
  if (data.length === 0) {
    return (
      <div className="rounded-lg border p-6 text-center text-sm text-muted-foreground">
        No data found.
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
      {data.map((item, index) => (
        <div
          key={index}
          className="rounded-lg border bg-background p-4 shadow-sm transition-shadow hover:shadow-md"
        >
          <div className="space-y-3">
            {config.columns.map((column) => (
              <div
                key={String(column.key)}
                className="space-y-1"
              >
                <div className="text-xs font-medium text-muted-foreground">
                  {column.label}
                </div>

                <div className="text-sm">
                  {column.render
                    ? column.render(item)
                    : String(item[column.key] ?? "-")}
                </div>
              </div>
            ))}
          </div>
        </div>
      ))}
    </div>
  );
}