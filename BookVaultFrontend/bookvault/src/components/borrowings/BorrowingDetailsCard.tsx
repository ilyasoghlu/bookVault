
import { Borrowing } from "@/types/borrowing";

import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

type Props ={
    borrowing: Borrowing;
}

function BorrowingDetailsCard({
    borrowing
}:Props){
    return(
        <Card>
            <CardHeader>
                <CardTitle className="font-bold text-2xl">
                    {borrowing.bookDetails?.title ?? "Not specified"}
                </CardTitle>
            </CardHeader>
            <CardContent className="space-y-2 grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                    <h3 className="font-bold text-xl mb-2"><strong>Reader Details:</strong></h3>
                    <h3><strong>Reader: </strong>{" "}{`${borrowing.readerDetails?.firstname ?? "Not specified"} ${borrowing.readerDetails?.lastname ?? "Not specified"}`}</h3>
                    <p><strong>Email: </strong>{" "} {borrowing.readerDetails?.email ?? "Not specified"}</p>
                    <p><strong>Phone: </strong>{" "}{borrowing.readerDetails?.phone ?? "Not specified"}</p>

                </div>
                <div>
                    <h3 className="font-bold text-xl mb-2"><strong>Borrowing Details:</strong></h3>
                    <p><strong>Duedate: </strong>{" "}{borrowing.duedate ?? "Not specified"}</p>
                    <p><strong>Return date: </strong>{" "}{borrowing.returndate ?? "Not specified"}</p>
                </div>
                <div>
                    <h3 className="font-bold text-xl mb-2"><strong>Creation Details:</strong></h3>
                    <p><strong>Borrow Created: </strong>{" "}{borrowing.createdat ?? "Not specified"}</p>
                    <p><strong>Created by: </strong>{" "}{borrowing.readerDetails?.createdby ?? "Not specified"}</p>
                </div>
                <div>
                    <h3 className="font-bold text-xl mb-2"><strong>Update Details:</strong></h3>
                    <p><strong>Borrow Updated: </strong>{" "}{borrowing.updatedat ?? "Not specified"}</p>
                    <p><strong>Updated by: </strong>{" "}{borrowing.readerDetails?.updatedby ?? "Not specified"}</p>
                </div>
                
                

                

                
                {/* <p><strong>Book created at: </strong>{" "}{borrowing.bookDetails?.createdat ?? "Not specified"}</p>
                <p><strong>Book updated at: </strong>{" "}{borrowing.bookDetails?.updatedat ?? "Not specified"}</p> */}
            </CardContent>
        </Card>
    )
}

export default BorrowingDetailsCard;