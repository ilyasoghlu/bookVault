

import { Reader } from '@/types/reader';

import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";


type Props = {
  reader: Reader;
};


function ReaderDetailsCard({
  reader, 
}: Props ) {
  return (
   <Card>
      <CardHeader>
        <CardTitle className="font-bold text-2xl">
          {reader.firstname} {reader.lastname}
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-3">
        <p>
          <strong>Birthday:</strong>{" "}
          {reader.dateofbirth}
        </p>
        <p>
          <strong>Member expiration date:</strong>{" "}
          {reader.expirationdate}
        </p>
        
          <p>
            <strong>Email:</strong>{" "}
            {reader.email}
          </p>
        
        <p>
          <strong>Phone:</strong>{" "}
          {reader.phone}
        </p>
    
     
      </CardContent>
    </Card>
  )
}

export default ReaderDetailsCard
