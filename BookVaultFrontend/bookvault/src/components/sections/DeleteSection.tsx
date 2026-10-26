'use client'

import { Button } from '../ui/button'
import { deleteSection } from '@/lib/sectionService';


type Props = {
  id:number;
  onDelete:(id:number) =>void;
};

function DeleteSectionButton({id, onDelete} : Props) {

  const handleDelete = async() =>{
    const confirmed = window.confirm(
      "Are you sure delete this section?"
    )
    
    if(!confirmed) return ;
    
    try {
      await deleteSection(id);
    
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

export default DeleteSectionButton
