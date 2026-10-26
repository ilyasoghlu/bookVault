import api from "./axios";

import {
  Book,
  CreateBookRequest,
  UpdateBookRequest,
} from "@/types/book";

export const getBooks = async (): Promise<Book[]> => {
  const response = await api.get<Book[]>("/api/book");

  return response.data;
};

export const getBookById = async (
  id: string
): Promise<Book> => {
  const response = await api.get<Book>(
    `/api/Book/${id}`
  );

  return response.data;
};

export const createBook = async (
  data: CreateBookRequest
): Promise<void> => {
  await api.post("/api/book", data);
};

export const updateBook = async (
  id: string,
  data: UpdateBookRequest
): Promise<void> => {
  await api.put(`/api/Book/${id}`, data);
};

export const deleteBook = async (
  id: string
): Promise<void> => {
  await api.delete(`/api/Book/${id}`);
};