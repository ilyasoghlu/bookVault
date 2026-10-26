
import { Shelf } from '@/types/shelf';


import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";


type Props = {
  shelf: Shelf;
};


function ShelfDetailsCard({
  shelf, 
}: Props ) {
  return (
   <Card>
      <CardHeader>
        <CardTitle>
          {shelf.shelvecode} 
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
          <strong>Capacity:</strong>{" "}
          {shelf.capacity}
        </p>
     
      </CardContent>
    </Card>
  )
}

export default ShelfDetailsCard;
