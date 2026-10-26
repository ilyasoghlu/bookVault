"use client"


import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getBookCaseById } from '@/lib/bookCaseService'
import BookCaseDetailsDialog from '@/components/bookCase/BookCaseDetailsDialog';
import { BookCase } from '@/types/bookcase';


function BookCaseDetailsPage() {

    const params = useParams()
    const id = params.id as string;
    const [bookCase, setBookCase] = useState<BookCase | null>(null)
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string |null>(null);
    const router = useRouter();
   
    useEffect(() =>{
        const fetchBookCase = async() =>{
          try {
            const data = await getBookCaseById(id);
            setBookCase(data)
          } catch (error) {
            console.error("Error", error);
            setError("Case information can't be found.")
          }finally{
            setLoading(false)
          }
        }
        fetchBookCase();
      }, [id]);

       if(loading) {
    return <div>Loading..</div>;
  }
  
  if(error || !bookCase) {
    return <div>{error ?? "Case not found."}</div>
  }
      return (
    <>
      <BookCaseDetailsDialog 
            bookCase={bookCase} 
            open={true} 
            onOpenChange={(open) => {
              if(!open) {
               router.push('/bookcases'); 
              }
            }} />
    </>
  )
}

export default BookCaseDetailsPage
