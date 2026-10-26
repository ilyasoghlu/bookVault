"use client"
import { Borrowing } from "@/types/borrowing";



import { 
    Card,
    CardContent,
    CardHeader,
    CardTitle
} from '../ui/card'

import { Button } from '../ui/button';
import { useAuth } from "@/hooks/useAuth";
import Link from 'next/link';

interface BorrowingCardProps{
  borrowing:Borrowing;
  onDelete:(id:number) => void
}

function BorrowingCard({borrowing}:BorrowingCardProps) {
  const{hasPermission} = useAuth();
  return (
     <Card>
      <CardHeader>
        <CardTitle className="font-bold text-2xl">
            {borrowing.bookDetails?.title ?? "Not specified"} 
        </CardTitle>
      </CardHeader>
      <CardContent>
        
        <h3>
            <strong>Reader: </strong>
            {`${borrowing.readerDetails?.firstname ?? "Not specified"} ${borrowing.readerDetails?.lastname ?? "Not specified"}`}
        </h3>

        <p>
            <strong>Duedate: </strong>
            {borrowing.duedate?? "Not specified"}
        </p>
        
        <p>
            <strong>Return date: </strong>
            {borrowing.returndate?? "Not specified"}
        </p>

        
        
        
          <Link href={`/borrowings/details/${borrowing.id}`}>
            <Button>
              View details
            </Button>
          </Link>
          {hasPermission("BORROWING_UPDATE")&&(
            <Link href={`/borrowings/edit/${borrowing.id}`}>
              <Button>
                Edit borrowing
              </Button>
            </Link>
          )}
  
      </CardContent>
    </Card>
  )
}

export default BorrowingCard
