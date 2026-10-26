import api from "./axios";


import { 
    Borrowing,
    CreateBorrowingRequest,
    UpdateBorrowingRequest
 } from "@/types/borrowing";

 export const getBorrowings = async(): Promise<Borrowing[]> =>{
    const response = await api.get<Borrowing[]>("/api/borrowing");
    return response.data
 }


 export const getBorrowingById = async(
    id:number
 ):Promise<Borrowing> =>{
    const response = await api.get<Borrowing>(
        `/api/Borrowing/${id}`
    )
    return response.data;
 }

 export const createBorrowing = async(
    data: CreateBorrowingRequest
 ): Promise<void> =>{
    await api.post("/api/borrowing/", data);
 }

 export const updateBorrowing = async(
    id:number,
    data:UpdateBorrowingRequest
 ):Promise<void> =>{
    await api.put(`/api/Borrowing/${id}`, data);
 }

