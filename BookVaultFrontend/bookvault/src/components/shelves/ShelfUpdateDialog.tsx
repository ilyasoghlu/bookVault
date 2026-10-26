"use client";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";
import { Shelf } from '@/types/shelf';
import CreateShelfForm from '@/components/shelves/CreateShelfForm';

type Props = {
  shelf: Shelf;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

export default function ShelfUpdateDialog({ 
    shelf, 
    open, 
    onOpenChange }: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreateShelfForm 
         mode="edit"
         shelf={shelf} />
        <DialogFooter>
          <DialogClose asChild>
            <button
                type="button"
                className="rounded-md border px-4 py-2 text-sm"
            >Close</button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}