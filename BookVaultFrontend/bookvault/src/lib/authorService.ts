import api from "./axios";
import {
  Author,
  CreateAuthorRequest,
  UpdateAuthorRequest,
} from "@/types/author";


export const getAuthors = async (): Promise<Author[]> => {
  const response = await api.get<Author[]>("/api/Authors");

  return response.data;
};


export const createAuthor = async (
  author: CreateAuthorRequest
): Promise<void> => {
  await api.post("/api/Authors", author);
};


export const updateAuthor = async (
  id: string,
  data: UpdateAuthorRequest
): Promise<void> => {

  await api.put(
    `/api/Authors/${id}`,
    data
  );

};
export const deleteAuthor = async (
  id: string
): Promise<void> => {

  await api.delete(
    `/api/Authors/${id}`
    
  );

};


export const getAuthorById = async (
  id: string
): Promise<Author> => {

  const response = await api.get<Author>(
    `/api/Authors/${id}`
  );

  return response.data;
};