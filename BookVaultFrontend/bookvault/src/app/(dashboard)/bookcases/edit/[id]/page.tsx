"use client"

import { useEffect, useState } from 'react'
import { useParams } from 'next/navigation'

import BookCaseUpdateDialog from '@/components/bookCase/BookCaseUpdateDialog'
import { getBookCaseById } from '@/lib/bookCaseService'
import { BookCase } from '@/types/bookcase'
import { useRouter } from 'next/navigation'



function EditBookCaseDetails() {
  const router = useRouter();
  const params = useParams();
  const id = params.id as string;

  const [bookCase, setBookCase] = useState<BookCase | null>(null);
  const [loading, setLoading] = useState(true);
 
  useEffect(() =>{
    const fetchBookCase = async() =>{
      try {
        const data = await getBookCaseById(id);
        setBookCase(data);
      } catch (error) {
        console.error("Error:", error)
      }finally{
        setLoading(false);
    }
  }
    fetchBookCase()
  }, [id]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!bookCase) {
    return <div>Case not found.</div>;
  }
  return (
    <BookCaseUpdateDialog
      bookCase={bookCase}
      open={true}
      onOpenChange={(open) => {
        if(!open) {
          router.push('/bookcases'); 
        }
      }}
        
    />
     
   
  )
}

export default EditBookCaseDetails
