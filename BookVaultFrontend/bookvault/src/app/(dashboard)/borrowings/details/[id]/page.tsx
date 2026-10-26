"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getBorrowingById } from "@/lib/borrowingService";
import BorrowingDetailsDialog from '@/components/borrowings/BorrowingDetailsDialog';
import { Borrowing } from '@/types/borrowing';


function BorrowingDetailsPage(){
   
const params = useParams()
  const id = Number(params.id);
  const router = useRouter();
  const [borrowing, setBorrowing] = useState<Borrowing | null>(null)
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string |null>(null);

  useEffect(() =>{
    const fetchBorrowing = async() =>{
      try {
        const data = await getBorrowingById(id);
        setBorrowing(data)
      } catch (error) {
        console.error("Error", error);
        setError("Borrowing information can't be found.")
      }finally{
        setLoading(false)
      }
    }
    fetchBorrowing();
  }, [id]);

  if(loading) {
    return <div>Loading..</div>;
  }
  
  if(error || !borrowing) {
    return <div>{error ?? "Gorrowing not found."}</div>
  }
    return(
      <BorrowingDetailsDialog 
        borrowing={borrowing} 
        open={true} 
        onOpenChange={(open) => {
        if (!open) {
          router.push('/borrowings');
        }
      }} />
        
    )
}

export default BorrowingDetailsPage;