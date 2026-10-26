"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";


import EmployeeDetailsCard from "./EmployeeDetailsCard";
import DeleteEmployeeButton from "./DeleteEmployeeButton";
import { Employee } from "@/types/employee";
import { useRouter } from "next/navigation";
import { DialogClose } from "@radix-ui/react-dialog";

type Props = {
  employee: Employee;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function EmployeeDetailsDialog({
  employee,
  open,
  onOpenChange,
}: Props) {
  const router = useRouter();
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <EmployeeDetailsCard employee={employee} />
         <DialogFooter>
        <button
          type="button"
          onClick={() => {
            router.push(`/employees/edit/${employee.id}`);
          }}
          className="rounded-md border px-4 py-2 text-sm"
        >
          Edit
        </button>
        <DeleteEmployeeButton id={employee.id} onDelete={() => {}} />
        
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

export default EmployeeDetailsDialog;