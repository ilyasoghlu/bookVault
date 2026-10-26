"use client";


import { Button } from "@/components/ui/button";
import { deleteAuthor } from "@/lib/authorService";

import { Trash2 } from "lucide-react";

type Props = {
  id: string;
  onDelete: (id:string) => void;
};

export default function DeleteAuthorButton({ id, onDelete }: Props) {
  

  const handleDelete = async () => {
    const confirmed = window.confirm(
      "Are you sure delete this author?"
  )

  

    if(!confirmed) return;
    
    try {
      await deleteAuthor(id);

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
      <Trash2 />
      Delete
    </Button>
  );
}