"use client";

import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";

import { Borrowing } from "@/types/borrowing";
import CreateBorrowingForm from "./CreateBorrowingForm";

type Props = {
  borrowing: Borrowing;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function BorrowingUpdateDialog({ 
    borrowing, 
    open,
    onOpenChange
 }: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreateBorrowingForm 
          mode="edit"
          borrowing={borrowing}   
        />
        <DialogFooter>
          <DialogClose asChild>
            <button 
              type="button"
              
              className="rounded-md border px-4 py-2 text-sm"
            >
              Close
            </button>
            </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}

export default BorrowingUpdateDialog;