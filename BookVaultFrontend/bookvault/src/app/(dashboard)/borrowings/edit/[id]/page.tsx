"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getBorrowingById } from "@/lib/borrowingService";
import { Borrowing } from '@/types/borrowing';
import BorrowingUpdateDialog from '@/components/borrowings/BorrowingUpdateDialog';



function EditBorrowingDetails(){
    const params = useParams()
    const id = Number(params.id)
    const router = useRouter();
    const [borrowing, setBorrowing] = useState<Borrowing | null>(null);
    const [loading, setLoading] = useState(true);
    
        useEffect(() => {
          const fetchBorrowing = async()=>{
            try {
              const data = await getBorrowingById(id);
              setBorrowing(data);
            } catch (error) {
              console.error("Error:", error);
    
            }finally{
              setLoading(false);
            }
          }
          fetchBorrowing()
        }, [id]);
       
         if (loading) {
        return <div>Loading...</div>;
      }
    
      if (!borrowing) {
        return <div>Borrowing not found.</div>;
      }

    return (
        <BorrowingUpdateDialog
          borrowing={borrowing}
          open={true}
          onOpenChange={(open) => {
            if(!open){
              router.push('/borrowings'); // Navigate back to the borrowings list
            }
          }}
        />
    )
}

export default EditBorrowingDetails;