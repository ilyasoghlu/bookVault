"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import PublisherUpdateDialog from '@/components/publishers/PublisherUpdateDialog'
import { getPublisherById } from '@/lib/publisherService'
import { Publisher } from '@/types/publisher'



function EditPublisherDetails() {
    const router = useRouter();
    const params = useParams()
    const id = params.id as string;
    const [publisher, setPublisher] = useState<Publisher | null>(null);
    const [loading, setLoading] = useState(true);
      
          useEffect(() => {
            const fetchPublisher = async()=>{
              try {
                const data = await getPublisherById(id);
                setPublisher(data);
              } catch (error) {
                console.error("Error:", error);
      
              }finally{
                setLoading(false);
              }
            }
            fetchPublisher()
          }, [id]);
         
           if (loading) {
          return <div>Loading...</div>;
        }
      
        if (!publisher) {
          return <div>Publisher not found.</div>;
        }

  return (
    <PublisherUpdateDialog
      publisher={publisher}
      open={true}
      onOpenChange={(open) => {
        if(!open){
          router.push('/publishers'); // Navigate back to the publishers list
        }
      }}
    /> 

  )
}

export default EditPublisherDetails
