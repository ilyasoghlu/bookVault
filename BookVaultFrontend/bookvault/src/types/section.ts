
import { BookCase } from "./bookcase";
import { Employee } from "./employee";


export interface Section{
    id: number;
    title:string;
    sectionnumber: number;
    capacity: number;
    sectionfloor:number;
    description:string;

    createdat?: string | null;
    updatedat?: string | null;

    employeeid?: number | null;
    roomid:number;
    
    bookCases?: BookCase[];
    employee?: Employee | null;
    
}


interface CreateSectionPayload{

    title:string;
    sectionnumber: number;
    capacity: number;
    sectionfloor:number;
    description:string;
    roomid:number;
}

interface UpdateSectionPayload{

    title:string;
    sectionnumber: number;
    capacity: number;
    sectionfloor:number;
    description:string;
    employeeid: number | null;
    roomid:number;
}

export type CreateSectionRequest = CreateSectionPayload;
export type UpdateSectionRequest = UpdateSectionPayload;