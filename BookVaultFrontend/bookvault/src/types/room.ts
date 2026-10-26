
import { Section } from "./section";
import { BookCase } from "./bookcase";
import { Employee } from "./employee";
import { Shelf } from "./shelf";

export interface Room{
    id: number;
    title:string;
    roomnumber:number;
    capacity: number;
    roomfloor:number;
    description:string;

    sectionid?: number | null;
    employeeid?: number | null;

    createdat?: string | null;
    updatedat?: string | null;

    section?: Section | null;
    employee?: Employee | null;
    bookCases?: BookCase[];
    shelves?: Shelf[];

}


interface CreateRoomPayload{
    title:string;
    roomnumber:number;
    capacity: number;
    roomfloor:number;
    description:string;
}
interface UpdateRoomPayload{
    title:string;
    roomnumber:number;
    capacity: number;
    description:string;
    employeeid: number | null;
}

export type CreateRoomRequest = CreateRoomPayload;
export type UpdateRoomRequest = UpdateRoomPayload;