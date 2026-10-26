import api from "./axios";

import { 
    Reader,
    CreateReaderRequest,
    UpdateReaderRequest
} from "@/types/reader";


export const getReaders = async() : Promise<Reader[]> =>{
    const response = await api.get<Reader[]>("/api/Reader");
    return response.data
}

export const createReader = async(
    reader : CreateReaderRequest
): Promise<void> =>{
    await api.post("/api/Reader", reader)
}

export const updateReader =async(
    id:number,
    data: UpdateReaderRequest
): Promise<void> =>{
    await api.put(`/api/Reader/${id}`, data)
}

export const deleteReader = async(
    id:number,
):Promise<void> =>{
    await api.delete(`/api/Reader/${id}`)
}

export const getReaderById = async(
    id:number,

): Promise<Reader> =>{
    const response = await api.get<Reader>(`/api/Reader/${id}`

    );
    return response.data;
}