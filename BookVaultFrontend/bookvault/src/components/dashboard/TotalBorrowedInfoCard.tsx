"use client"

import { useEffect, useState } from "react"
import { Borrowing } from "@/types/borrowing"
import { Card, CardContent } from "../ui/card"
import { BookOpenCheck } from "lucide-react"
import { getBorrowings } from "@/lib/borrowingService"


function TotalBorrowedInfoCard() {

  const [borrowings, setBorrowings] = useState<Borrowing[]>([])

  useEffect(() => {
    const fetchBorrowings = async () => {
      const data = await getBorrowings()
      setBorrowings(data)
    }
    fetchBorrowings();
  }, []);

  const totalBorrowed = borrowings.length;

  return (
    <Card>
      <CardContent className="flex flex-row items-center justify-center space-x-3">
        <div className="p-4 bg-blue-100 rounded-xl">
          <BookOpenCheck />
        </div>
        <div className="flex flex-col items-center">
          <h4 className="text-2xl font-bold">
             Borrowed Book{totalBorrowed > 1 && 's'}
          </h4>
          <h4 className="text-2xl font-bold text-blue-500">
            {totalBorrowed}
          </h4>
        </div>
      </CardContent>
    </Card>
  )
}

export default TotalBorrowedInfoCard
