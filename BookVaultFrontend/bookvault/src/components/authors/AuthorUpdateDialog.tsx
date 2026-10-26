"use client"

import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";

import { Author } from "@/types/author";
import CreateAuthorForm from "./CreateAuthorForm";

type Props = {
  author: Author;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};


function AuthorUpdateDialog({ 
    author, 
    open,
    onOpenChange
 }: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreateAuthorForm 
          mode="edit"
          author={author}   
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

export default AuthorUpdateDialog;
