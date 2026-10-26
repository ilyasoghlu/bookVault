import { Section } from "./section";
import { Room } from "./room";
import { Shelf } from "./shelf";

export interface BookCase{
    id: number;
    casenumber: number;
    capacity: number;

    sectionid?: number | null;
    roomid?: number | null;

    createdat?: string | null;
    updatedat?: string | null;

    section?: Section | null;
    room?: Room | null;
    shelves?: Shelf[];

}


interface BookCasePayload {
    casenumber: number;
    capacity: number;

    sectionid?: number | null;
    roomid?: number | null;
} 

export type CreateBookCaseRequest = BookCasePayload;
export type UpdateBookCaseRequest = BookCasePayload;