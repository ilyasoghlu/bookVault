"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogClose
} from "@/components/ui/dialog";

import ReaderDetailsCard from "./ReaderDetailsCard";
import DeleteReaderButton from "./DeleteReader";
import { Reader } from "@/types/reader";
import { useRouter } from "next/navigation";




type Props = {
  reader: Reader;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function ReaderDetailsDialog({
  reader,
  open,
  onOpenChange
}: Props) {
  const router = useRouter();
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <ReaderDetailsCard reader={reader} />
         <DialogFooter>
        <button
          type="button"
          onClick={() => {
            router.push(`/readers/edit/${reader.id}`);
          }}
          className="rounded-md border px-4 py-2 text-sm"
        >
          Edit
        </button>
        <DeleteReaderButton id={reader.id} onDelete={() => {}} />
        
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

export default ReaderDetailsDialog;