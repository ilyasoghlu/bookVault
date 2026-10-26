"use client"

import { Book } from "@/types/book"
import DeleteBookButton from "./DeleteBookButton"

import { 
    Card,
    CardContent,
    CardHeader,
    CardTitle
} from '../ui/card'

import { Button } from '../ui/button';
import Link from 'next/link';
import { useAuth } from "@/hooks/useAuth";

interface BookCardProps{
  book:Book;
  onDelete:(id:string) => void
}

function BookCard({book, onDelete}:BookCardProps) {

  const {hasPermission} = useAuth();
  return (
     <Card>
      <CardHeader>
        <CardTitle>
            <h1 className="font-bold ">{book.title}</h1>
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
            <strong>Category: </strong>
            {book.category}
        </p>
        <p>
            <strong>Genre: </strong>
            {book.genre}
        </p>
        
        <p>
          <strong>Author: </strong>
          {book.author
            ? `${book.author.firstname} ${book.author.lastname}`
            : "Not specified"}
        </p>

        <p>
          <strong>Publisher: </strong>
          {book.publisher
            ? book.publisher.publishername
            : "Not specified"}
        </p>
        
         <p>
          <strong>Total count:</strong>{" "}
          {book.totalcopies}
        </p>

        <p>
          <strong>Status: </strong>
          {book.borrowing
            ? `${book.borrowing.availableCopies} available / ${book.borrowing.totalCopies} total`
            : "No data"}
        </p>

        
          <Link href={`/books/details/${book.id}`}>
            <Button>
              View details
            </Button>
          </Link>
           {hasPermission("BOOK_UPDATE") && (
          <Link href={`/books/edit/${book.id}`}>
            <Button>
              Edit book
            </Button>
          </Link>
        )}

        {hasPermission("BOOK_DELETE") && (
          <DeleteBookButton
            id={book.id}
            onDelete={onDelete}
          />
        )}
      </CardContent>
    </Card>
  )
}

export default BookCard
