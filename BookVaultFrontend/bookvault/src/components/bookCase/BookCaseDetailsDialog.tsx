"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogClose,
} from "@/components/ui/dialog";

import BookCaseDetailsCard from "./BookCaseDetailsCard";
import { BookCase } from "@/types/bookcase"; 
import DeleteBookCaseButton from "./DeleteBookCaseButton";

import { useRouter } from "next/navigation";

type Props = {
  bookCase: BookCase;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function BookCaseDetailsDialog({
  bookCase,
  open,
  onOpenChange
}: Props) {

  const router = useRouter();
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <BookCaseDetailsCard bookCase={bookCase} />
         <DialogFooter>
        <button
          type="button"
          onClick={() => {
            router.push(`/bookcases/edit/${bookCase.id}`);
          }}
          className="rounded-md border px-4 py-2 text-sm"
        >
          Edit
        </button>
        <DeleteBookCaseButton id={bookCase.id} onDelete={() => {}} />
        
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

export default BookCaseDetailsDialog;