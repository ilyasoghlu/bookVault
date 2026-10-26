"use client";

import { 
    Dialog, 
    DialogContent,
    DialogFooter,
    DialogClose
} from "@/components/ui/dialog";

import CreateBookCaseForm from "./CreateBookCaseForm";
import { BookCase } from "@/types/bookcase";

type Props = {
  bookCase: BookCase;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function BookCaseUpdateDialog({ 
    bookCase, 
    open,
    onOpenChange
 }: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreateBookCaseForm 
          mode="edit"
          bookCase={bookCase}   
        />
        <DialogFooter>
            <DialogClose asChild>
                <button
                  type="button"
                  className="rounded-md border px-4 py-2"
                >
                  Close
                </button>
            </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

export default BookCaseUpdateDialog;