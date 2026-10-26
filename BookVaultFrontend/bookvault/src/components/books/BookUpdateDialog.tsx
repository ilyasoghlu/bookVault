"use client";   

import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";

import { Book } from '@/types/book';
import CreateBookForm from '@/components/books/CreateBookForm';

type Props = {
  book: Book;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

export default function BookUpdateDialog({ 
    book, 
    open, 
    onOpenChange 
}: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreateBookForm mode="edit" book={book} />
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
  );
}


