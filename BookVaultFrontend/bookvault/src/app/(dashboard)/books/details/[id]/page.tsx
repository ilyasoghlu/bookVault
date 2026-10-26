"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getBookById } from "@/lib/bookService";
import { Book } from '@/types/book';
import BookDetailsDialog from '@/components/books/BookDetailsDialog';



export default function BookDetailsPage() {
    const params = useParams()
    const id = params.id as string;
    const router = useRouter();
    const [book, setBook] = useState<Book | null>(null)
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string |null>(null);

     useEffect(() =>{
        const fetchBook = async() =>{
          try {
            const data = await getBookById(id);
            setBook(data)
          } catch (error) {
            console.error("Error", error);
            setError("Book information can't be found.")
          }finally{
            setLoading(false)
          }
        }
        fetchBook();
      }, [id]);

      if(loading) {
    return <div>Loading..</div>;
  }
  
  if(error || !book) {
    return <div>{error ?? "Book not found."}</div>
  }
  return (
    <BookDetailsDialog 
        book={book} 
        open={true} 
        onOpenChange={(open) => {
          if (!open) {
            router.push('/books');
          }
        }}
        
        />

  );
}