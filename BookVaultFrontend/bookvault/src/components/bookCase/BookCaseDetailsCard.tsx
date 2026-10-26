

import { BookCase } from "@/types/bookcase";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";


type Props = {
  bookCase: BookCase;
};


export default function BookCaseDetailsCard({
  bookCase,
}: Props) {

  return (
    <Card>

      <CardHeader>
        <CardTitle>
          {bookCase.casenumber}
        </CardTitle>
      </CardHeader>


      <CardContent className="space-y-3">
        <p>
          <strong>Case number: </strong>{" "}
          {bookCase.casenumber}
        </p>

        <p>
          <strong>Capacity: </strong>{" "}
          {bookCase.capacity}
        </p>

      </CardContent>
    </Card>
  );
}