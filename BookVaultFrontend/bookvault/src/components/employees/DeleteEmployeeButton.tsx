'use client'

import { Button } from '../ui/button'
import { deleteEmployee } from '@/lib/employeeService'


type Props = {
  id:number;
  onDelete:(id:number) =>void;
};

function DeleteEmployeeButton({id, onDelete} : Props) {

  const handleDelete = async() =>{
    const confirmed = window.confirm(
      "Are you sure delete this employee"
    )
    
    if(!confirmed) return ;
    
    try {
      await deleteEmployee(id);
    
      onDelete(id);
    } catch (err) {
      console.error(err);
    }
  }; 


  return (
    <Button
      variant="destructive"
      onClick={handleDelete}
    >
      Delete
    </Button>
  )
}

export default DeleteEmployeeButton
