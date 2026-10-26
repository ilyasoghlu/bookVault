
"use client";

import { Button } from "../ui/button";
import { deleteShelf } from "@/lib/shelfService";
import axios from "axios";

type Props = {
  id: number;
  onDelete: (id: number) => void;
};

function DeleteShelfButton({ id, onDelete }: Props) {
  const handleDelete = async () => {
    const confirmed = window.confirm(
      "Are you sure you want to delete this shelf?"
    );

    if (!confirmed) return;

    try {
      await deleteShelf(id);

      onDelete(id);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        const message =
          err.response?.data?.message ??
          "Failed to delete shelf.";

        window.alert(message);
      } else {
        window.alert("Failed to delete shelf.");
      }
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

export default DeleteShelfButton;

