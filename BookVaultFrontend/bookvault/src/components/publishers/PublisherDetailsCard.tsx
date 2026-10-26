

import { Publisher } from "@/types/publisher";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";


type Props = {
  publisher: Publisher;
};


export default function PublisherDetailsCard({
  publisher,
}: Props) {
  return (
    <Card>
      <CardHeader>
        <CardTitle  >
          <h1 className="font-bold">
            {publisher.publishername} 
          </h1>
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-6">
        <div className="space-4-2">
          <p>
            <strong>Publisher address:</strong>{" "}
            {publisher.address}
          </p>

          <p>
            <strong>City:</strong>{" "}
            {publisher.city}
          </p>
          
          <p>
            <strong>Website:</strong>{" "}
            {publisher.website}
          </p>
          
          <p>
            <strong>Email:</strong>{" "}
            {publisher.email}
          </p>
          
          <p>
            <strong>Phone:</strong>{" "}
            {publisher.phone}
          </p>
        </div>
         <div className="space-y-4">
          <h3 className="text-lg font-semibold">
            Authors & Books
          </h3>
          

          {publisher.authors &&
          publisher.authors.length > 0 ? (
            publisher.authors.map((author) => {
              const authorBooks =
                publisher.books?.filter(
                  (book) =>
                    book.authorid === author.id &&
                    book.publisherid === publisher.id
                ) ?? [];

              return (
                <div
                  key={author.id}
                  className="space-y-2 border-b pb-4 last:border-b-0"
                >
                  <p className="font-semibold">
                    {author.firstname} {author.lastname}
                  </p>

                  {authorBooks.length > 0 ? (
                    <ul className="list-disc pl-5 space-y-1">
                      {authorBooks.map((book) => (
                        <li key={book.id}>
                          {book.title}
                        </li>
                      ))}
                    </ul>
                  ) : (
                    <p className="text-sm text-muted-foreground">
                      No books found for this author from this publisher.
                    </p>
                  )}
                </div>
              );
            })
          ) : (
            <p className="text-muted-foreground">
              No authors found for this publisher.
            </p>
          )}
        </div>
      </CardContent>
    </Card>
  );
}