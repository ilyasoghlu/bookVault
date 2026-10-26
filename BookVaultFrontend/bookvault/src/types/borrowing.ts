export interface BorrowingReaderDetails {
    id: number;

    firstname: string;
    lastname: string;
    email: string;
    phone: string;

    dateofbirth: string;
    expirationdate: string;

    createdat?: string | null;
    updatedat?: string | null;

    createdby: string;
    updatedby: string;
}

export interface BorrowingBookDetails {
    id: string;

    isbn: string;
    title: string;
    category: string;
    genre: string;

    publicationdate: string;
    pagecount: number;
    edition: string;
    totalcopies: number;
    language: string;

    createdat?: string | null;
    updatedat?: string | null;
}

export interface Borrowing {
    id: number;

    duedate: string;
    returndate: string | null;

    createdat?: string | null;
    updatedat?: string | null;

    readerid: number;
    readerDetails: BorrowingReaderDetails | null;

    bookid: string;
    bookDetails: BorrowingBookDetails | null;
}

export interface CreateBorrowingRequest {
    duedate: string;
    readerid: number;
    bookid: string;
}

export interface UpdateBorrowingRequest {
    duedate: string;
    returndate?: string | null;
}