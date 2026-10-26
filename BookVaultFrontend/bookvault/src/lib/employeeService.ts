import api from "./axios";

import { 
    Employee,
    CreateEmployeeRequest,
    UpdateEmployeeRequest
} from "@/types/employee";


export const getEmployees = async(): Promise<Employee[]> =>{
    const response = await api.get<Employee[]>("/api/Employee");
    return response.data;
}

export const createEmployee = async(
    employee:CreateEmployeeRequest
)=>{
    await api.post("/api/Employee", employee)
}

export const updateEmployee = async(
    id:number,
    data: UpdateEmployeeRequest
): Promise<void> =>{
    await api.put(
        `/api/Employee/${id}`, data
    );
};

export const deleteEmployee = async (
    id:number
): Promise<void> =>{
    await api.delete(
        `/api/Employee/${id}`
    )
}


export const getEmployeeById = async (
    id:number
): Promise<Employee> =>{
    const response = await api.get<Employee>(
        `/api/Employee/${id}`
    )
    return response.data;
}