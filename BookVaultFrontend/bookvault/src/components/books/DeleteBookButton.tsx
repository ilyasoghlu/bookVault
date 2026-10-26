"use client"

import { Button } from "../ui/button"
import { deleteBook } from "@/lib/bookService"


type Props = {
  id: string;
  onDelete:(id:string) =>void;
};

function DeleteBookButton({id, onDelete} : Props) {

  const handleDelete = async() =>{
    const confirmed = window.confirm(
      "Are you sure delete this book?"
    )

     if(!confirmed) return ;
        
        try {
          await deleteBook(id);
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

export default DeleteBookButton
