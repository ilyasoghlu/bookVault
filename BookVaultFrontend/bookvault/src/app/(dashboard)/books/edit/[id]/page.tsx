"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import BookUpdateDialog from '@/components/books/BookUpdateDialog'
import { getBookById } from '@/lib/bookService'
import { Book } from '@/types/book';



function UpdateBookDetails() {

  const params = useParams();
  const router = useRouter();
  const id = params.id as string;
  const [book, setBook ] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
        const fetchBook = async()=>{
          try {
            const data = await getBookById(id);
            setBook(data);
          } catch (error) {
            console.error("Error:", error);
  
          }finally{
            setLoading(false);
          }
        }
        fetchBook()
      }, [id]);
     
       if (loading) {
      return <div>Loading...</div>;
    }
  
    if (!book) {
      return <div>Book not found.</div>;
    }

  return (
    <BookUpdateDialog 
      book={book} 
      open={true} 
      onOpenChange={(open) => {
        if(!open){
        router.push('/books'); 
      }
    }} />
     
    
  )
}

export default UpdateBookDetails

