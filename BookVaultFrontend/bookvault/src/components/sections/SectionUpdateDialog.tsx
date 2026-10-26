"use client";

import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";

import { Section } from '@/types/section';
import CreateSectionForm from '@/components/sections/CreateSectionForm';

type Props = {
  section: Section;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

export default function SectionUpdateDialog({ 
    section, 
    open, 
    onOpenChange }: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreateSectionForm 
         mode="edit"
         section={section} />
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
