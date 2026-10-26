"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogClose
} from "@/components/ui/dialog";

import ShelfeDetailsCard from "./ShelfeDetailsCard";
import { Shelf } from "@/types/shelf";
import { useRouter } from "next/navigation";
import DeleteShelfButton from "./DeleteShelf"; 


type Props = {
  shelf: Shelf;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function ShelfDetailsDialog({
  shelf,
  open,
  onOpenChange
}: Props) {
  const router = useRouter();
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <ShelfeDetailsCard shelf={shelf} />
         <DialogFooter>
        <button
          type="button"
          onClick={() => {
            router.push(`/shelves/edit/${shelf.id}`);
          }}
          className="rounded-md border px-4 py-2 text-sm"
        >
          Edit
        </button>
        <DeleteShelfButton id={shelf.id} onDelete={() => {}} />
        
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

export default ShelfDetailsDialog;