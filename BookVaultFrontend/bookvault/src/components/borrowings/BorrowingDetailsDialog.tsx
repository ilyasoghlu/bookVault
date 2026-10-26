"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogClose,
} from "@/components/ui/dialog";


import BorrowingDetailsCard from "./BorrowingDetailsCard";
import { Borrowing } from "@/types/borrowing";
import { useRouter } from "next/navigation";


type Props = {
  borrowing: Borrowing;
  open: boolean;
  onOpenChange: (open: boolean) => void;

};

function BorrowingDetailsDialog({
  borrowing,
  open,
  onOpenChange,
}: Props) {
  const router = useRouter();
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <BorrowingDetailsCard borrowing={borrowing} />
         <DialogFooter>
        <button
          type="button"
          onClick={() => {
            router.push(`/borrowings/edit/${borrowing.id}`);
          }}
          className="rounded-md border px-4 py-2 text-sm"
        >
          Edit
        </button>
      
        
        <DialogClose asChild>
          <button 
            type="button" 
            onClick={() => onOpenChange(false)}
            className="rounded-md border px-4 py-2 text-sm"
            >
              Close
          </button>
        </DialogClose>
      </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

export default BorrowingDetailsDialog;