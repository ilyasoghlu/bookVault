import api from "./axios";

import { 
    Shelf,
    CreateShelfRequest,
    UpdateShelfRequest
 } from "@/types/shelf";

 export const getShelves = async() :Promise<Shelf[]> =>{
    const response = await api.get<Shelf[]>("/api/shelf")

    return response.data
 }

 export const createShelf = async(
    shelf:CreateShelfRequest 
 ): Promise<void> =>{
    await api.post("/api/shelf", shelf)
 }

 export const updateShelf = async(
    id:number,
    data: UpdateShelfRequest
 ): Promise<void> =>{
    await api.put(`/api/shelf/${id}`, data )
 }


 export const deleteShelf = async (
    id:number
 ): Promise<void> =>{
    await api.delete(`/api/shelf/${id}`)
 }

 export const getShelfById = async(
    id:number,
 ):Promise<Shelf> =>{
    const response = await api.get<Shelf>(`/api/shelf/${id}`)
    return response.data
 }