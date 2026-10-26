"use client";

import { useEffect, useState } from "react";
import { Card, CardContent } from "../ui/card";
import { BookOpen } from "lucide-react";

import { Book } from "@/types/book";
import { getBooks } from "@/lib/bookService";

function TotalBooksInfoCard() {
  const [books, setBooks] = useState<Book[]>([]);

  useEffect(() => {
    const fetchBooks = async () => {
      try {
        const data = await getBooks();
        setBooks(data);
      } catch (error) {
        console.error("Books could not be loaded:", error);
      }
    };

    fetchBooks();
  }, []);

  const totalBooks = books.length;

  return (
    <Card>
      <CardContent className="flex flex-row items-center justify-center space-x-3">
        <div className="rounded-xl bg-blue-100 p-4">
          <BookOpen />
        </div>

        <div className="flex flex-col items-center">
          <h4 className="text-2xl font-bold">
            Total Book{totalBooks !== 1 && "s"}
          </h4>

          <h4 className="text-2xl font-bold text-blue-500">
            {totalBooks}
          </h4>
        </div>
      </CardContent>
    </Card>
  );
}

export default TotalBooksInfoCard;