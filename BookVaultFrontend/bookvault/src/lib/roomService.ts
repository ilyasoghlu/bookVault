import api from "./axios";



import { 
    Room,
    CreateRoomRequest,
    UpdateRoomRequest
} from "@/types/room";


export const getRooms = async() : Promise<Room[]> =>{
    const response = await api.get<Room[]>("/api/Room");
    return response.data
}

export const createRoom = async(
    room : CreateRoomRequest
): Promise<void> =>{
    await api.post("/api/Room", room)
}

export const updateRoom =async(
    id:number,
    data: UpdateRoomRequest
): Promise<void> =>{
    await api.put(`/api/Room/${id}`, data)
}

export const deleteRoom = async(
    id:number,
):Promise<void> =>{
    await api.delete(`/api/Room/${id}`)
}

export const getRoomById = async(
    id:number,

): Promise<Room> =>{
    const response = await api.get<Room>(`/api/Room/${id}`

    );
    return response.data;
}