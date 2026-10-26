'use client'

import { Button } from '../ui/button'
import { deleteReader } from '@/lib/readerService';

type Props = {
  id:number;
  onDelete:(id:number) =>void;
};

function DeleteReaderButton({id, onDelete} : Props) {

  const handleDelete = async() =>{
    const confirmed = window.confirm(
      "Are you sure delete this reader?"
    )
    
    if(!confirmed) return ;
    
    try {
      await deleteReader(id);
    
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

export default DeleteReaderButton
