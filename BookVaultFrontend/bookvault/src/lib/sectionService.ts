
import api from "./axios";

import { 
    Section,
    CreateSectionRequest,
    UpdateSectionRequest
} from "@/types/section";


export const getSections = async() :Promise<Section[]> =>{
    const response = await api.get<Section[]>("/api/section")

    return response.data
}

export const createSection = async (
    section: CreateSectionRequest
):Promise<void> =>{
    await api.post("/api/section", section)
}

export const updateSection = async(
    id:number,
    data:UpdateSectionRequest
): Promise<void> => {
    await api.put(`/api/Section/${id}`, data)
}

export const deleteSection = async(
    id:number,
):Promise<void>=>{
    await api.delete(`/api/Section/${id}`)
}

export const getSectionById = async(
    id:number,
): Promise<Section> => {
    const response = await api.get<Section>(`/api/Section/${id}`)
    return response.data
}