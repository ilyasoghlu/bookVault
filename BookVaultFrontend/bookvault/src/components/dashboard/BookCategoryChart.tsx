"use client"

import { 
  Pie,
  PieChart,
  Label
} from "recharts"

import { 
  Card, 
  CardContent, 
  CardHeader, 
  CardTitle 
} from "../ui/card"

import {
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
  type ChartConfig
} from "../ui/chart"

import {Book} from "@/types/book";

type Props = {
  books: Book[]
}

function BookCategoryChart({ books }: Props) {
  const categoryCounts : Record<string, number> = {};

  books.forEach(book => {
    categoryCounts[book.category] = (categoryCounts[book.category] || 0) + 1;
  });

  const chartData = Object.entries(categoryCounts).map(([category, count]) => ({  
    category,
    count
  }))
    
    console.log(categoryCounts)
  return (
    <div className="border-2 p-4 rounded-xl">
      Book Category Chart Component
    </div>
  )
}

export default BookCategoryChart
