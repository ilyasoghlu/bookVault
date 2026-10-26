
import { Room } from '@/types/room';

import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";


type Props = {
  room: Room;
};


function RoomDetailsCard({
  room, 
}: Props ) {
  return (
   <Card>
      <CardHeader>
        <CardTitle>
          {room.roomnumber} 
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-3">
        <p>
          <strong>Floor:</strong>{" "}
          {room.roomfloor}
        </p>
        <p>
          <strong>Capacity:</strong>{" "}
          {room.capacity}
        </p>
        <p>
          <strong>Description:</strong>{" "}
          {room.description}
        </p>
     
      </CardContent>
    </Card>
  )
}

export default RoomDetailsCard;
