import { z } from "zod";

export const authorSchema = z.object({
  firstname: z
    .string()
    .min(2, "Firstname cant be les than minimum 2 symbol")
    .max(30, "Firstname cannot exceed 30 characters"),

  lastname: z
    .string()
    .min(2, "Lastname cant be les than minimum 2 symbol")
    .max(30, "Lastname cannot exceed 30 characters"),

  birthdate: z
    .string()
    .min(1, "Birthdate required"),

  deathdate: z.string().nullable().optional(),


  nationality: z
    .string()
    .min(2, "Nationality required")
    .max(30, "Nationality cannot exceed 30 characters"),

  biography: z
    .string()
    .min(10, "Biography cant be less than 10 symbols" ),

  publisherid:z
    .string()
    .nullable()
    .optional(),
});

export type AuthorFormValues = z.infer<typeof authorSchema>;