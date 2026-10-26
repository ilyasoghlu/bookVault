"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getPublisherById } from '@/lib/publisherService';
import PublisherDetailsDialog from '@/components/publishers/PublisherDetailsDialog';
import { Publisher } from '@/types/publisher';



 function PublisherDetailsPage() {

  const params = useParams()
  const id = params.id as string;
  const router = useRouter();
  const [publisher, setPublisher] = useState<Publisher | null>(null)
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string |null>(null);

  useEffect(() =>{
    const fetchPublisher = async() =>{
      try {
        const data = await getPublisherById(id);
        setPublisher(data)
      } catch (error) {
        console.error("Error", error);
        setError("Publisher information can't be found.")
      }finally{
        setLoading(false)
      }
    }
    fetchPublisher();
  }, [id]);

  if(loading) {
    return <div>Loading..</div>;
  }
  
  if(error || !publisher) {
    return <div>{error ?? "Publisher not found."}</div>
  }

  return (
     <PublisherDetailsDialog 
        publisher={publisher} 
        open={true} 
        onOpenChange={(open) => {
       if (!open) {
         router.push('/publishers');
       }
     }} />
    
  )
}

export default PublisherDetailsPage