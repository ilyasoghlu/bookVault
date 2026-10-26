"use client";

import { useEffect, useState } from "react";
import { useAuth } from "@/hooks/useAuth";
import Link from "next/link";

import { getBorrowings } from "@/lib/borrowingService";
import { Borrowing } from "@/types/borrowing";
import BorrowingCard from "@/components/borrowings/BorrowingCard";
import DataView from "@/components/data-view/DataView";
import { DataViewConfig } from "@/components/data-view/types";
import { Button } from "@/components/ui/button";

export default function BooksPage() {
  const [borrowings, setBorrowings] = useState<Borrowing[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const {hasPermission} = useAuth();
  const totalBorrowing = borrowings.length;

  const borrowingConfig: DataViewConfig<Borrowing> = {
    columns: [
      {
        key: "bookid",
        label: "Book",
        render: (borrowings:Borrowing) => 
          borrowings.bookDetails?.title ??  "Not specified",
      },
      {
        key: "readerid",
      label: "Reader",
      render: (borrowing: Borrowing) =>
        `${borrowing.readerDetails?.firstname ?? "Not specified"} ${
          borrowing.readerDetails?.lastname ?? "Not specified"
        }`,
      },
      {
        key: "duedate",
        label: "Duedate"
      },
      {
        key: "id",
        label: " Actions",
        render: (borrowings: Borrowing) => (
           <div className="flex items-center gap-2">
          {/* Details */}
          <Link href={`/borrowings/details/${borrowings.id}`}>
            <Button size="sm">
              View details
            </Button>
          </Link>

      {/* Edit */}
      {hasPermission("BORROWING_UPDATE") && (
        <Link href={`/borrowings/edit/${borrowings.id}`}>
          <Button size="sm" variant="outline">
            Edit
          </Button>
        </Link>
      )}
      </div>
        )
      }

    ]
  }

  const handleDelete = (id: number) => {
    setBorrowings((prev) => prev.filter((borrowing) => borrowing.id!==id));
  };

  useEffect(() => {
    const fetchBorrowings = async () => {
      try {
        const data = await getBorrowings();
        setBorrowings(data);
      } catch (error) {
        console.error("Error:", error);
        setError("Borrowings' information can't be found.");
      } finally {
        setLoading(false);
      }
    };

    fetchBorrowings();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (

    
    <div className="container mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        
        <h4 className="text-2xl font-bold">{totalBorrowing} Borrow{totalBorrowing >1 && 's'} found</h4>
          {hasPermission("BORROWING_CREATE")&&(
            <Link href="/borrowings/create">
            <button
              type="button"
              className="
                rounded-md 
                outline 
                hover:outline-blue-300 
                 hover:text-orange-600
                border 
                px-3 
                py-2 
                text-sm"
              >
              Create Borrowing
            </button>
          </Link>
          )}
      </div>

      {borrowings.length === 0 ? (
        <p>No borrowings found.</p>
      ) : (
        <DataView
            data ={borrowings}
            config = {borrowingConfig}
            renderGridItem={(borrowing) =>(
              <BorrowingCard 
                borrowing ={borrowing}
               onDelete={handleDelete}
              />
            )} 
             
            />
          )}
    </div>
  );
}