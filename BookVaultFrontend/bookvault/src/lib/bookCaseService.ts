


import api from "./axios";
import { 
    BookCase,
    CreateBookCaseRequest, 
    UpdateBookCaseRequest 
} from './../types/bookcase';



export const getBookCase = async() :Promise<BookCase []> =>{
    const response = await api.get<BookCase[]>("/api/BookCase")

    return response.data
}


export const createBookCase = async (
    bookCase: CreateBookCaseRequest
): Promise<void> =>{
    await api.post("/api/BookCase", bookCase);
};


export const updateBookCase = async (
    id:number,
    data: UpdateBookCaseRequest
): Promise<void> =>{
    await api.put(`/api/BookCase/${id}`, data);
};

export const deleteBookCase = async (
    id:number
):Promise<void> => {
    await api.delete(
        `/api/BookCase/${id}`,
    );
};


export const getBookCaseById = async (
  id: string
): Promise<BookCase> => {

  console.log("CASE ID:", id);

  const response = await api.get<BookCase>(
    `/api/BookCase/${id}`
  );

  return response.data;
};