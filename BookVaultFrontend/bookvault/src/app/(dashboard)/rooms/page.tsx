
"use client";

import { useEffect, useState } from "react";
import { getRooms } from "@/lib/roomService";
import { Room } from "@/types/room";
import RoomCard from "@/components/rooms/RoomCard";
import { useAuth } from "@/hooks/useAuth";
import { Button } from "@/components/ui/button";
import DataView from "@/components/data-view/DataView";
import { DataViewConfig } from "@/components/data-view/types";
import DeleteRoomButton from "@/components/rooms/DeleteRoom";
import Link from "next/link";


export default function RoomsPage() {
  const [room, setRoom] = useState<Room[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const {hasPermission} = useAuth();
  const totalRooms = room.length;
  
  const roomConfig:DataViewConfig<Room> = {
    columns: [
      {
        key: "title",
        label: "Title",
      },
      {
        key: "roomnumber",
        label: "Room number",
      },
      {
        key: "roomfloor",
        label: "Room floor",
      },
      {
        key:"id",
        label:"Actions",
        render: (room:Room) =>(
          <div className="flex items-center gap-2">
          {/* Details */}
          <Link href={`/rooms/details/${room.id}`}>
            <Button size="sm">
              View details
            </Button>
          </Link>

      {/* Edit */}
      {hasPermission("ROOM_UPDATE") && (
        <Link href={`/rooms/edit/${room.id}`}>
          <Button size="sm" variant="outline">
            Edit
          </Button>
        </Link>
      )}

      {/* Delete */}
      {hasPermission("ROOM_DELETE") && (
        <DeleteRoomButton
          id={room.id}
          onDelete={handleDelete}
        />
      )}
    </div>
        )
      }

    ]
  }

  const handleDelete = (id:number) =>{
    setRoom((prev)=> prev.filter((room) => room.id !==id))
  }
  
  useEffect(() => {
    const fetchRooms = async () => {
      try {
        const data = await getRooms();
        setRoom(data);
      } catch (error) {
        console.error("Error:", error);
        setError("Readers' information cant found.")
      }finally{
        setLoading(false)

      };
    };

    fetchRooms();
  }, []);

  if (loading) {
    return <h2>Loading...</h2>;
    } if (error) {
    return <h2>{error}</h2>;
     }
  return (
    <div className="container mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        <h4 className="text-2xl font-bold">{totalRooms} Room{totalRooms > 1&&'s'} found</h4>
        {hasPermission("ROOM_CREATE")&&(
          <Link href="/rooms/create">
            <button
              type="button"
              className="
                rounded-md 
                outline 
                hover:outline-blue-300 
                hover:text-orange-600   
                border 
                px-3 
                py-2 
                text-sm"
              >
                Create room
            </button>
          </Link>
        )}  
      </div>
    
     {room.length === 0 ? (
      <p>No rooms found</p>
     ): (
      <DataView
        data={room}
        config={roomConfig}
        renderGridItem={(room) =>(
          <RoomCard
          room={room}
          onDelete={handleDelete}
          />
        )}
      />
      
     )}
    </div>
    
  );
}

