"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getReaderById } from '@/lib/readerService'
import { Reader } from '@/types/reader';
import ReaderUpdateDialog from '@/components/readers/ReaderUpdateDialog';



function EditReaderDetails() {

  const router = useRouter();   
  const params = useParams()
  const id = Number(params.id)
  const [reader, setReader] = useState<Reader | null>(null);
  const [loading, setLoading] = useState(true);

      useEffect(() => {
        const fetchReader = async()=>{
          try {
            const data = await getReaderById(id);
            setReader(data);
          } catch (error) {
            console.error("Error:", error);
  
          }finally{
            setLoading(false);
          }
        }
        fetchReader()
      }, [id]);
     
       if (loading) {
      return <div>Loading...</div>;
    }
  
    if (!reader) {
      return <div>Reader not found.</div>;
    }  

  return (
    <ReaderUpdateDialog
      reader={reader}
      open={true}
      onOpenChange={(open) => {
        if(!open){
          router.push('/readers'); // Navigate back to the readers list
        }
      }}
    />

  )
}

export default EditReaderDetails
