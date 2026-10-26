import { Borrowing } from "./borrowing";

export interface Reader {
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

    borrowings?: Borrowing[];
}

interface ReaderPayload{
    firstname: string;
    lastname: string;
    email: string;
    phone: string;

    dateofbirth: string;
    expirationdate: string;
}

export interface CreateReaderRequest extends ReaderPayload {
    username: string;
    password: string;
}


export type UpdateReaderRequest = ReaderPayload;