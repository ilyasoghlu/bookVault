"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogClose
} from "@/components/ui/dialog";

import SectionDetailsCard from "./SectionDetailsCard";
import { useRouter } from "next/navigation";
import { Section } from "@/types/section";  
import DeleteSectionButton from "./DeleteSection"; 


type Props = {
  section: Section;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function SectionDetailsDialog({
  section,
  open,
  onOpenChange
}: Props) {
  const router = useRouter();
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <SectionDetailsCard section={section} />
         <DialogFooter>
        <button
          type="button"
          onClick={() => {
            router.push(`/sections/edit/${section.id}`);
          }}
          className="rounded-md border px-4 py-2 text-sm"
        >
          Edit
        </button>
        <DeleteSectionButton id={section.id} onDelete={() => {}} />
        
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

export default SectionDetailsDialog;