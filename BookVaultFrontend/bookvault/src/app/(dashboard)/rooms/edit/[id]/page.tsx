"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import RoomUpdateDialog from '@/components/rooms/RoomUpdateDialog'
import { getRoomById } from '@/lib/roomService'
import { Room } from '@/types/room';



function EditRoomDetails() {
  const router = useRouter();   
  const params = useParams()
  const id = Number(params.id)
  const [room, setRoom] = useState<Room | null>(null);
  const [loading, setLoading] = useState(true);
  
      useEffect(() => {
        const fetchRoom = async()=>{
          try {
            const data = await getRoomById(id);
            setRoom(data);
          } catch (error) {
            console.error("Error:", error);
  
          }finally{
            setLoading(false);
          }
        }
        fetchRoom()
      }, [id]);
     
       if (loading) {
      return <div>Loading...</div>;
    }
  
    if (!room) {
      return <div>Room not found.</div>  
    }
  return (
    <RoomUpdateDialog
      room={room}
      open={true}
      onOpenChange={(open) => {
        if(!open){
          router.push('/rooms'); // Navigate back to the rooms list
        }
      }}
    />

  )
}

export default EditRoomDetails
