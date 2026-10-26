"use client";   
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";
import { Reader } from "@/types/reader";
import CreateReaderForm from "./CreateReaderForm";

type Props = {
  reader: Reader;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function ReaderUpdateDialog({
  reader,
  open,
  onOpenChange
}: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreateReaderForm
          mode="edit"
          reader={reader}
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

export default ReaderUpdateDialog;