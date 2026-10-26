import { Author } from './author';
import { Publisher } from './publisher';
import { Shelf } from './shelf';

export interface ActiveBorrowing{
    borrowedBy?: string |null;
    dueDate?: string | null;
}

export interface BookBorrowing {
    totalCopies: number;
    borrowedCopies: number;
    availableCopies:number;
    activeBorrowings: ActiveBorrowing[];
}

export interface BookLocation {
    sectionTitle?: string | null;
    roomNumber?: number | null;
    roomFloor?: number | null;
    caseNumber?: number | null;
    shelfCode?: number | number;
    
    employeeName?: string | null;
    employeePosition?: string |null;
}

export interface Book {
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
    
    authorid?: string | null;
    publisherid?: string | null;
    shelfid?: number | null;

    author?: Author | null;
    publisher?: Publisher | null;
    shelf?: Shelf | null;
    location?: BookLocation | null;
    borrowing?: BookBorrowing | null;
}

export interface BookPayload{
    isbn:string;
    title: string;
    category: string;
    genre: string;
    publicationdate: string;
    edition: string;
    language: string;
    
    pagecount: number;
    totalcopies: number;
    
    authorid: string;
    publisherid: string;
    shelfid: number;
}


export type CreateBookRequest = BookPayload;
export type UpdateBookRequest = BookPayload;