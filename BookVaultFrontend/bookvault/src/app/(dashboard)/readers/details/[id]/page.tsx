"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getReaderById } from '@/lib/readerService';
import ReaderDetailsDialog from '@/components/readers/ReaderDetailsDialog';
import { Reader } from '@/types/reader';



function ReaderDetailsPage() {
   const params = useParams()
    const id = Number(params.id);
    const router = useRouter();
    const [reader, setReader] = useState<Reader | null>(null)
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string |null>(null);
  
    useEffect(() =>{
      const fetchReader = async() =>{
        try {
          const data = await getReaderById(id);
          setReader(data)
        } catch (error) {
          console.error("Error", error);
          setError("Reader information can't be found.")
        }finally{
          setLoading(false)
        }
      }
      fetchReader();
    }, [id]);
  
    if(loading) {
      return <div>Loading..</div>;
    }
    
    if(error || !reader) {
      return <div>{error ?? "Reader not found."}</div>
    }
  return (
    <>
     <ReaderDetailsDialog 
        reader={reader} 
        open={true} 
        onOpenChange={(open) => {
       if (!open) {
         router.push('/readers');
       }
     }} />
    </>
  )
}

export default ReaderDetailsPage