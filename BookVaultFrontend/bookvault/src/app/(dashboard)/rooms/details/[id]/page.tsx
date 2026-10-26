"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getRoomById } from '@/lib/roomService';
import RoomDetailsDialog from '@/components/rooms/RoomDetailsDialog';
import { Room } from '@/types/room';



function RoomDetailsPage() {
  const params = useParams()
    const id = Number(params.id);
    const router = useRouter();
    const [room, setRoom] = useState<Room | null>(null)
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string |null>(null);
  
    useEffect(() =>{
      const fetchRoom = async() =>{
        try {
          const data = await getRoomById(id);
          setRoom(data)
        } catch (error) {
          console.error("Error", error);
          setError("Room information can't be found.")
        }finally{
          setLoading(false)
        }
      }
      fetchRoom();
    }, [id]);
  
    if(loading) {
      return <div>Loading..</div>;
    }
    
    if(error || !room) {
      return <div>{error ?? "Room not found."}</div>
    }

  return (
    <>
     <RoomDetailsDialog 
        room={room} 
        open={true} 
        onOpenChange={(open) => {
          if (!open) {
            router.push('/rooms');
          }
        }} 
     />
    </>
  )
}

export default RoomDetailsPage