"use client";

import { DataViewProps } from "./types";

export default function DataList<T>({
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
    <div className="w-full overflow-x-auto rounded-lg border">
      <table className="w-full min-w-max">
        <thead>
          <tr className="border-b bg-muted/50">
            {config.columns.map((column) => (
              <th
                key={String(column.key)}
                className="px-4 py-3 text-left text-sm font-medium bg-blue-400"
              >
                {column.label}
              </th>
            ))}
          </tr>
        </thead>

        <tbody className="">
          {data.map((item, index) => (
            <tr
              key={index}
              className="border-b last:border-0 hover:bg-blue-300  even:bg-blue-200"
            >
              {config.columns.map((column) => (
                <td
                  key={String(column.key)}
                  className="px-4 py-3 text-sm"
                >
                  {column.render
                    ? column.render(item)
                    : String(item[column.key] ?? "-")}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}