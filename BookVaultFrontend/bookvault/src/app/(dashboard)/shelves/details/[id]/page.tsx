"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getShelfById } from '@/lib/shelfService';
import ShelfDetailsDialog from '@/components/shelves/ShelfDetailsDialog';
import { Shelf } from '@/types/shelf';



function ShelfDetailsPage() {
    const params = useParams()
    const id = Number(params.id);
    const router = useRouter();
    const [shelf, setShelf] = useState<Shelf | null>(null)
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string |null>(null);
  
    useEffect(() =>{
      const fetchShelf = async() =>{
        try {
          const data = await getShelfById(id);
          setShelf(data)
        } catch (error) {
          console.error("Error", error);
          setError("Shelf information can't be found.")
        }finally{
          setLoading(false)
        }
      }
      fetchShelf();
    }, [id]);
  
    if(loading) {
      return <div>Loading..</div>;
    }
    
    if(error || !shelf) {
      return <div>{error ?? "Shelf not found."}</div>
    }
    return(
    <>
     <ShelfDetailsDialog 
        shelf={shelf} 
        open={true} 
        onOpenChange={(open) => {
          if (!open) {
            router.push('/shelves');
          }
        }} 
     />
    </>
  )
}

export default ShelfDetailsPage