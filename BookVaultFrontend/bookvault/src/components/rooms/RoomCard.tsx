"use client"
import { Room } from '@/types/room';
import DeleteRoomButton from './DeleteRoom';

import { 
    Card,
    CardContent,
    CardHeader,
    CardTitle
} from '../ui/card'

import { Button } from '../ui/button';
import { useAuth } from '@/hooks/useAuth';
import Link from 'next/link';

interface RoomCardProps{
    room :Room;
    onDelete:(id:number) =>void
}

function RoomCard({room, onDelete} : RoomCardProps) {
  const {hasPermission} = useAuth();
  return (
     <Card>
      <CardHeader>
        <CardTitle>
            {room.roomnumber} 
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
            <strong>Floor: {room.roomfloor}</strong>
        </p>
        <p>
            <strong>Capacity: {room.capacity}</strong>
        </p>
        <p>
            <strong>Description: {room.description}</strong>
        </p>
        
          <Link href={`/rooms/details/${room.id}`}>
            <Button>
              View details
            </Button>
          </Link>
          {hasPermission("ROOM_UPDATE")&&(
            <Link href={`/rooms/edit/${room.id}`}>
              <Button>
                Edit room
              </Button>
            </Link>
          )}
          {hasPermission("ROOM_DELETE")&&(
            <DeleteRoomButton 
              id={room.id} 
              onDelete={onDelete}
              // fullname={`${author.firstname} ${author.lastname}`} 
              />
          )}
          
      </CardContent>
    </Card>
  )
}

export default RoomCard
