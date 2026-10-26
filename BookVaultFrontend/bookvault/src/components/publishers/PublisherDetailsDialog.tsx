"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogClose,  
} from "@/components/ui/dialog";

import PublisherDetailsCard from "./PublisherDetailsCard";
import DeletePublisherButton from "./DeletePublisherButton";
import { useRouter } from "next/navigation";
import {Publisher} from "@/types/publisher";

type Props = {
  publisher: Publisher;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function PublisherDetailsDialog({
  publisher,
  open,
  onOpenChange
}: Props) {
  const router = useRouter();
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <PublisherDetailsCard publisher={publisher} />
         <DialogFooter>
        <button
          type="button"
          onClick={() => {
            router.push(`/publishers/edit/${publisher.id}`);
          }}
          className="rounded-md border px-4 py-2 text-sm"
        >
          Edit
        </button>
        <DeletePublisherButton id={publisher.id} onDelete={() => {}} />
        
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

export default PublisherDetailsDialog;