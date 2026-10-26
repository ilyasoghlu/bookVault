"use client";


import { Button } from "@/components/ui/button";
import { deletePublisher } from "@/lib/publisherService";


type Props = {
  id: string;
  onDelete: (id:string) => void;
};

export default function DeletePublisherButton({ id, onDelete }: Props) {
  

  const handleDelete = async () => {
    const confirmed = window.confirm(
      "Are you sure delete this publisher?"
  )

    if(!confirmed) return;
    
    try {
      await deletePublisher(id);

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
  );
}