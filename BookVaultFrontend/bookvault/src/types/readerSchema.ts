import { z } from "zod";

const readerFields = {
  firstname: z
    .string()
    .min(2, "Firstname must contain at least 2 characters")
    .max(30, "Firstname can't exceed 30 characters"),

  lastname: z
    .string()
    .min(2, "Lastname must contain at least 2 characters")
    .max(30, "Lastname can't exceed 30 characters"),

  email: z
    .string()
    .email("Please enter a valid email address")
    .max(30, "Email can't exceed 30 characters"),

  phone: z
    .string()
    .min(10, "Phone number is too short")
    .max(20, "Phone number is too long"),

  dateofbirth: z
    .string()
    .min(1, "Date of birth is required"),

  expirationdate: z
    .string()
    .min(1, "Expiration date is required"),
};

export const readerSchema = z.object({
  ...readerFields,

  username: z.string().optional(),

  password: z.string().optional(),
});

export type ReaderFormValues = z.infer<typeof readerSchema>;