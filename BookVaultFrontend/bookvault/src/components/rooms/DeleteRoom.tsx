'use client'

import { Button } from '../ui/button'
import { deleteRoom } from '@/lib/roomService';

type Props = {
  id:number;
  onDelete:(id:number) =>void;
};

function DeleteRoomButton({id, onDelete} : Props) {

  const handleDelete = async() =>{
    const confirmed = window.confirm(
      "Are you sure delete this room?"
    )
    
    if(!confirmed) return ;
    
    try {
      await deleteRoom(id);
    
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

export default DeleteRoomButton
