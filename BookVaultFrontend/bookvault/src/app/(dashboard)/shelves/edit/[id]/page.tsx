"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import ShelfUpdateDialog from '@/components/shelves/ShelfUpdateDialog'
import { getShelfById } from '@/lib/shelfService'
import { Shelf } from '@/types/shelf';



function EditShelfDetails() {

  const params = useParams()
  const id = Number(params.id)
  const router = useRouter();
  const [shelf, setShelf] = useState<Shelf | null>(null);
  const [loading, setLoading] = useState(true);
  
      useEffect(() => {
        const fetchShelf = async()=>{
          try {
            const data = await getShelfById(id);
            setShelf(data);
          } catch (error) {
            console.error("Error:", error);
  
          }finally{
            setLoading(false);
          }
        }
        fetchShelf()
      }, [id]);
     
       if (loading) {
      return <div>Loading...</div>;
    }
  
    if (!shelf) {
      return <div>Shelf not found.</div>;
    }  

  return (
    <ShelfUpdateDialog 
        shelf={shelf} 
        open={true} 
        onOpenChange={(open) => {
          if (!open) {
            router.push('/shelves');
          }
        }} 
      />

  )
}

export default EditShelfDetails
