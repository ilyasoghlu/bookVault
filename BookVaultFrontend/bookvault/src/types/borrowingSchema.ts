import { z } from "zod";

export const createBorrowingSchema = z.object({
  duedate: z
    .string()
    .min(1, "Due date is required"),

  readerid: z.coerce
    .number()
    .int()
    .positive("Reader is required"),

  bookid: z
    .string()
    .uuid("Book id must be a valid UUID"),
});

export const updateBorrowingSchema = z.object({
  duedate: z
    .string()
    .min(1, "Due date is required"),

  returndate: z
    .string()
    .nullable()
    .optional(),
});

export type CreateBorrowingFormInput =
  z.input<typeof createBorrowingSchema>;

export type CreateBorrowingFormValues =
  z.output<typeof createBorrowingSchema>;

export type UpdateBorrowingFormInput =
  z.input<typeof updateBorrowingSchema>;

export type UpdateBorrowingFormValues =
  z.output<typeof updateBorrowingSchema>;