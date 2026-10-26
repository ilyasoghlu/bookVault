"use client";   
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";
import { Publisher } from "@/types/publisher";
import CreatePublisherForm from "./CreatePublisherForm";

type Props = {
  publisher: Publisher;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function PublisherUpdateDialog({
  publisher,
  open,
  onOpenChange
}: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreatePublisherForm
          mode="edit"
          publisher={publisher}
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

export default PublisherUpdateDialog;
