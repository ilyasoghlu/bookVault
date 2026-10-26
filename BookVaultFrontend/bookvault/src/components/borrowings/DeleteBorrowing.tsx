"use client"

import { Button } from "../ui/button"
import { deleteBorrowing } from "@/lib/borrowingService";


type Props = {
  id: number;
  onDelete:(id:number) =>void;
};

function DeleteBorrowingButton({id, onDelete} : Props) {

  const handleDelete = async() =>{
    const confirmed = window.confirm(
      "Are you sure delete this book?"
    )

     if(!confirmed) return ;
        
        try {
          await deleteBorrowing(id);
          onDelete(id);
        } catch (err) {
          console.error(err);
        }
  }
  return (
    <Button
      variant="destructive"
      onClick={handleDelete}
    >
      Delete
    </Button>
  )
}

export default DeleteBorrowingButton
