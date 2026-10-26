"use client"

import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
} from "@/components/ui/dialog";
import { Room } from "@/types/room";
import CreateRoomForm from "./CreateRoomForm";

type Props = {
  room: Room;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function RoomUpdateDialog({
  room,
  open,
  onOpenChange
}: Props) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <CreateRoomForm
          mode="edit"
          room={room}
        />
        <DialogFooter>
          <DialogClose asChild>
            <button
              type="button"
              className="rounded-md border px-4 py-2 text-sm"
            >
              Close
            </button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}

export default RoomUpdateDialog;