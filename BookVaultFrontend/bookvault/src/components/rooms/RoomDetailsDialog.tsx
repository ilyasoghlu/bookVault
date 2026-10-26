"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogClose
} from "@/components/ui/dialog";

import RoomDetailsCard from "./RoomDetailsCard";
import DeleteRoomButton from "./DeleteRoom";
import { Room } from "@/types/room";   
import { useRouter } from "next/navigation";


type Props = {
  room: Room;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

function RoomDetailsDialog({
  room,
  open,
  onOpenChange
}: Props) {
  const router = useRouter();
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <RoomDetailsCard room={room} />
         <DialogFooter>
        <button
          type="button"
          onClick={() => {
            router.push(`/rooms/edit/${room.id}`);
          }}
          className="rounded-md border px-4 py-2 text-sm"
        >
          Edit
        </button>
        <DeleteRoomButton id={room.id} onDelete={() => {}} />
        
        <DialogClose asChild>
          <button 
            type="button" 
            onClick={() => onOpenChange(false)}
            className="rounded-md border px-4 py-2 text-sm"
            >
              Close
          </button>
        </DialogClose>
      </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

export default RoomDetailsDialog;