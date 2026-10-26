"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";

import BookDetailsCard from "./BookDetailsCard";
import DeleteBookButton from "./DeleteBookButton";
import { Book } from "@/types/book";
import { useRouter } from "next/navigation";
import { DialogClose } from "@radix-ui/react-dialog";


type Props = {
  book: Book;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function BookDetailsDialog({
  book,
  open,
  onOpenChange,
}: Props) {
  const router = useRouter();
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <BookDetailsCard book={book} />
      <DialogFooter>
        <button
          type="button"
          onClick={() => {
            router.push(`/books/edit/${book.id}`);
          }}
          className="rounded-md border px-4 py-2 text-sm"
        >
          Edit
        </button>
        <DeleteBookButton id={book.id}  onDelete={() =>{}}/>
      
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

export default BookDetailsDialog;