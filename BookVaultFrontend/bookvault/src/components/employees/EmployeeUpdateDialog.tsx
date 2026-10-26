"use client";   
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";
import { Employee } from "@/types/employee";
import CreateEmployeeForm from "./CreateEmployeeForm";  

type Props = {  
  employee: Employee;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function EmployeeUpdateDialog({ 
    employee, 
    open,
    onOpenChange
 }: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreateEmployeeForm 
          mode="edit"
          employee={employee}   
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

export default EmployeeUpdateDialog;