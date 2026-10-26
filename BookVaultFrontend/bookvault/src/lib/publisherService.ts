import api from "./axios";

import { 
    Publisher,
    CreatePublisherRequest,
    UpdatePublisherRequest
} from "@/types/publisher";


export const getPublishers = async() : Promise<Publisher[]>=>{
    const response = await api.get<Publisher[]>("/api/Publishers");

    return response.data
}


export const createPublisher = async(
    publisher : CreatePublisherRequest
): Promise<void> =>{
    await api.post("/api/Publishers", publisher)
}


export const updatePublisher = async(
    id:string,
    data : UpdatePublisherRequest
): Promise<void> =>{
    await api.put(`/api/Publishers/${id}`, data)
}


export const deletePublisher = async(
    id:string,
   
): Promise<void> =>{
    await api.delete(`/api/Publishers/${id}`)
}



export const getPublisherById = async(
    id:string,
   
): Promise<Publisher> =>{
    const response = await api.get<Publisher>(`/api/Publishers/${id}`

    );
    return response.data;
}