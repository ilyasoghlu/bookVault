"use client";

import * as React from "react";
import * as RechartsPrimitive from "recharts";

import { cn } from "@/lib/utils";

export type ChartConfig = {
  [key: string]: {
    label?: React.ReactNode;
    color?: string;
    icon?: React.ComponentType;
  };
};

type ChartContextProps = {
  config: ChartConfig;
};

const ChartContext = React.createContext<ChartContextProps | null>(null);

function useChart() {
  const context = React.useContext(ChartContext);

  if (!context) {
    throw new Error("useChart must be used within a <ChartContainer />");
  }

  return context;
}

type ChartContainerProps = {
  config: ChartConfig;
  children: React.ComponentProps<typeof RechartsPrimitive.ResponsiveContainer>["children"];
  className?: string;
};

function ChartContainer({
  config,
  children,
  className,
}: ChartContainerProps) {
  return (
    <ChartContext.Provider value={{ config }}>
      <div
        data-chart="chart"
        className={cn(
          "flex aspect-video justify-center text-xs",
          className
        )}
      >
        <RechartsPrimitive.ResponsiveContainer width="100%" height="100%">
          {children}
        </RechartsPrimitive.ResponsiveContainer>
      </div>
    </ChartContext.Provider>
  );
}

type ChartTooltipProps = React.ComponentProps<
  typeof RechartsPrimitive.Tooltip
>;

function ChartTooltip(props: ChartTooltipProps) {
  return <RechartsPrimitive.Tooltip {...props} />;
}

type ChartTooltipContentProps = {
  active?: boolean;
  payload?: Array<{
    name?: string;
    value?: number | string;
    dataKey?: string;
    payload?: Record<string, unknown>;
  }>;
  label?: string;
  hideLabel?: boolean;
  hideIndicator?: boolean;
};

function ChartTooltipContent({
  active,
  payload,
  label,
  hideLabel = false,
  hideIndicator = false,
}: ChartTooltipContentProps) {
  const { config } = useChart();

  if (!active || !payload?.length) {
    return null;
  }

  return (
    <div className="grid min-w-32 gap-2 rounded-lg border bg-background p-2 shadow-xl">
      {!hideLabel && label && (
        <div className="font-medium">
          {label}
        </div>
      )}

      <div className="grid gap-1.5">
        {payload.map((item, index) => {
          const key = item.dataKey ?? item.name ?? `item-${index}`;
          const configItem = config[key];

          return (
            <div
              key={`${key}-${index}`}
              className="flex items-center justify-between gap-4"
            >
              <div className="flex items-center gap-2">
                {!hideIndicator && (
                  <span
                    className="h-2 w-2 shrink-0 rounded-full"
                    style={{
                      backgroundColor:
                        configItem?.color ?? "currentColor",
                    }}
                  />
                )}

                <span className="text-muted-foreground">
                  {configItem?.label ?? item.name ?? key}
                </span>
              </div>

              <span className="font-medium tabular-nums">
                {item.value}
              </span>
            </div>
          );
        })}
      </div>
    </div>
  );
}

export {
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
};