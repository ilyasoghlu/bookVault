"use client";

import { useState } from "react";
import { ReactNode } from "react";
import { LayoutGrid, List} from "lucide-react";
import DataList from "./DataList";
import { DataViewConfig } from "./types";

type DataViewProps<T> = {
  data: T[];
  config: DataViewConfig<T>;
  renderGridItem: (item: T) => ReactNode;
};

type ViewMode = "list" | "grid";

export default function DataView<T>({
  data,
  config,
  renderGridItem,
}: DataViewProps<T>) {
  const [viewMode, setViewMode] = useState<ViewMode>("list");

  return (
    <div className="w-full">

      {/* Desktop */}
      <div className="hidden md:block">

        <div className="mb-4 flex justify-end gap-2">
          <button
            type="button"
            onClick={() => setViewMode("list")}
            className={`rounded-md border px-3 py-2 text-sm ${
              viewMode === "list"
                ? "bg-muted font-medium"
                : ""
            }`}
          >
            <List />
          </button>

          <button
            type="button"
            onClick={() => setViewMode("grid")}
            className={`rounded-md border px-3 py-2 text-sm ${
              viewMode === "grid"
                ? "bg-muted font-medium"
                : ""
            }`}
          >
            <LayoutGrid />
          </button>
        </div>

        {viewMode === "list" ? (
          <DataList
            data={data}
            config={config}
          />
        ) : (
          <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
            {data.map((item, index) => (
              <div key={index}>
                {renderGridItem(item)}
              </div>
            ))}
          </div>
        )}

      </div>

      {/* Mobile */}
      <div className="block md:hidden">
        <div className="grid grid-cols-1 gap-6">
          {data.map((item, index) => (
            <div key={index}>
              {renderGridItem(item)}
            </div>
          ))}
        </div>
      </div>

    </div>
  );
}