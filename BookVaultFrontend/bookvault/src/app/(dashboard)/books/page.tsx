"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

import { getBooks } from "@/lib/bookService";
import { Book } from "@/types/book";
import BookCard from "@/components/books/BookCard";
import DataView from "@/components/data-view/DataView";
import { Button } from "@/components/ui/button";
import DeleteBookButton from "@/components/books/DeleteBookButton";
import { DataViewConfig } from "@/components/data-view/types";

import { useAuth } from "@/hooks/useAuth";

export default function BooksPage() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true); 
  const [error, setError] = useState<string | null>(null);

  const {hasPermission} = useAuth();

  const totalBooks = books.length;

  const bookConfig: DataViewConfig<Book> = {
  columns: [
    {
      key: "title",
      label: "Title",
    },
    {
      key: "category",
      label: "Category",
    },
    {
      key: "genre",
      label: "Genre",
    },
    {
      key: "author",
      label: "Author",
      render: (book: Book) =>
        book.author
          ? `${book.author.firstname} ${book.author.lastname}`
          : "Not specified",
    },
    {
      key: "publisher",
      label: "Publisher",
      render: (book: Book) =>
        book.publisher?.publishername ?? "Not specified",
    },
    {
      key: "totalcopies",
      label: "Total copies",
      
    },
    {
      key: "borrowing",
      label: "Status",
      render: (book: Book) =>
         book.borrowing
            ? `${book.borrowing.availableCopies} available / ${book.borrowing.totalCopies}`
            : "No data",
    },
    {
      key: "id",
      label: "Actions",
      render: (book: Book) => (
        <div className="flex items-center gap-2">
          {/* Details */}
          <Link href={`/books/details/${book.id}`}>
            <Button size="sm">
              View details
            </Button>
          </Link>

      {/* Edit */}
      {hasPermission("BOOK_UPDATE") && (
        <Link href={`/books/edit/${book.id}`}>
          <Button size="sm" variant="outline">
            Edit
          </Button>
        </Link>
      )}

      {/* Delete */}
      {hasPermission("BOOK_DELETE") && (
        <DeleteBookButton
          id={book.id}
          onDelete={handleDelete}
        />
      )}
    </div>
  ),
},
  ],
};

  const handleDelete = (id: string) => {
    setBooks((prev) => prev.filter((book) => book.id !==id));
  };

  useEffect(() => {
    const fetchBooks = async () => {
      try {
        const data = await getBooks();
        setBooks(data);
      } catch (error) {
        console.error("Error:", error);
        setError("Books' information can't be found.");
      } finally {
        setLoading(false);
      }
    };

    fetchBooks();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  

  return (
    <div className="mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        <h4 className="text-2xl font-bold">
            {totalBooks} Book{totalBooks > 1&&'s'} found
          </h4>
          {hasPermission("BOOK_CREATE") && (
            <Link href="/books/create">
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
                Create Book
              </button>
            </Link>
          )}
      </div>

       {books.length === 0 ? (
        <p>No books found.</p>
      ) : (
        <DataView
          data={books}
          config={bookConfig}
          renderGridItem={(book) => (
            <BookCard
              book={book}
              onDelete={handleDelete}
            />
          )}
        />
      )}
    </div>
  );
}