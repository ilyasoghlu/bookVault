import { Book } from "@/types/book";

import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

type Props = {
  book: Book;
}

function BookDetailsCard({book}: Props) {
  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-center">
          <h1 className="text-2xl font-bold">
            {book.title}
          </h1>
          </CardTitle>
      </CardHeader>
      <CardContent className="space-y-2">
        <p className="mb-4">
          <strong className="font-semibold">ISBN:</strong>{" "}
          {book.isbn}
        </p>
        <p >
          <strong>Author:</strong>{" "}
          {book.author?.firstname} {book.author?.lastname}
        </p>
        <p>
          <strong >Publisher:</strong>{" "}
          {book.publisher?.publishername}
        </p>
        <p>
          <strong>Category:</strong>{" "}
          {book.category}
        </p>
        <p>
          <strong>Genre:</strong>{" "}
          {book.genre}
        </p>
        <p>
          <strong>Edition:</strong>{" "}
          {book.edition}
        </p>
        <p>
          <strong>Language:</strong>{" "}
          {book.language}
        </p>
        <p>
          <strong>Publication date:</strong>{" "}
          {book.publicationdate}
        </p>
        <p>
          <strong>Page count:</strong>{" "}
          {book.pagecount}
        </p>
        <p>
          <strong>Section:</strong>{" "}
          {book.location?.sectionTitle ?? "Not specified"}
        </p>
        <p>
          <strong>Floor:</strong>{" "}
          {book.location?.roomFloor ?? "Not specified"}
        </p>
        <p>
          <strong>Room:</strong>{" "}
          {book.location?.roomNumber ?? "Not specified"}
        </p>
        <p>
          <strong>Book case:</strong>{" "}
          {book.location?.caseNumber ?? "Not specified"}
        </p>
        <p>
          <strong>Shelf:</strong>{" "}
          {book.location?.shelfCode ?? "Not specified"}
        </p>
        <p>
          <strong>Total count:</strong>{" "}
          {book.totalcopies}
        </p>
        <p>
          <strong>Status: </strong>
          {book.borrowing?.availableCopies }  available
      
         
        </p>
        {/* <p>
          <strong>Status: </strong>
          {book.borrowing
            ? `${book.borrowing.availableCopies}
            //  available / ${book.borrowing.totalCopies} total
             `
            : "No data"}
        </p> */}

        {book.borrowing && book.borrowing.activeBorrowings.length > 0 && (
          <>
            <p>
              <strong>Borrowed copies:</strong>{" "}
              {book.borrowing.borrowedCopies}
            </p>

            <div>
              <strong>Current borrowers:</strong>

              {book.borrowing.activeBorrowings.map((borrowing, index) => (
                <p key={index}>
                  {borrowing.borrowedBy} — Due date: {borrowing.dueDate}
                </p>
              ))}
            </div>
          </>
        )}
        
        <p>
          <strong>Employee:</strong>{" "}
          {book.location?.employeeName ?? "Not specified"}
        </p>
        <p>
          <strong>Employee position:</strong>{" "}
          {book.location?.employeePosition ?? "Not specified"}
        </p>
      </CardContent>
    </Card>
  )
}

export default BookDetailsCard
