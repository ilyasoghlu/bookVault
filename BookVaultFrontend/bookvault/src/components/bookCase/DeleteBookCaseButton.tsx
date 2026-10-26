"use client";


import { Button } from "@/components/ui/button";
import { deleteBookCase } from "@/lib/bookCaseService";

type Props = {
  id: number;
  onDelete: (id:number) => void;
};

export default function DeleteBookCaseButton({ id, onDelete }: Props) {
  const handleDelete = async () => {
    const confirmed = window.confirm(
      "Are you sure delete this author?"
  )

    if(!confirmed) return;
    
    try {
      await deleteBookCase(id);

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