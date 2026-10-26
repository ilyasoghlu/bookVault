import { Room } from "./room";
import { Section } from "./section";


export interface Employee{
    id:number;
    firstname: string;
    lastname:string;
    email:string;
    phone:string;
    employeeposition:string;

    hiredate:string;
    enddate?:string |null;

    createdat?:string | null;
    updatedat?:string | null;

    rooms?: Room[];
    sections?:Section[];
    
}

interface EmployeePayload{
    firstname: string;
    lastname:string;
    email:string;
    phone:string;
    employeeposition:string;

    hiredate:string;
    enddate?:string |null;
}

export interface CreateEmployeeRequest extends EmployeePayload {
    username: string;
    password: string;
}


export type UpdateEmployeeRequest = EmployeePayload;