import { Author } from "@/types/author";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";


type Props = {
  author: Author;
};


export default function AuthorDetailsCard({
  author,
}: Props) {

  return (
    <Card>

      <CardHeader>
        <CardTitle>
          {author.firstname} {author.lastname}
        </CardTitle>
      </CardHeader>


      <CardContent className="space-y-3">

        <p>
          <strong>Nationality:</strong>{" "}
          {author.nationality}
        </p>


        <p>
          <strong>Birth:</strong>{" "}
          {author.birthdate}
        </p>
       

        {author.deathdate && (
          <p>
            <strong>Death:</strong>{" "}
            {author.deathdate}
          </p>
        )}


        <p>
          {author.biography}
        </p>
        <p>
          {author.createdat}
        </p>
        <h3>Publishers & Books </h3>
        {
          
        }

      </CardContent>

    </Card>
  );
}