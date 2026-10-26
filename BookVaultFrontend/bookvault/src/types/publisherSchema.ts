
import {z} from "zod";



export const publisherSchema = z.object({
  publishername: z
    .string()
    .min(2, "Publisher name must contain at least 2 characters")
    .max(30, "Publisher name cannot exceed 30 characters"),

  address: z
    .string()
    .min(2, "Address must contain at least 2 characters")
    .max(50, "Address cannot exceed 50 characters"),

  city: z
    .string()
    .min(2, "City must contain at least 2 characters")
    .max(30, "City cannot exceed 30 characters"),

 website: z
  .string()
  .min(2, "Website is required")
  .max(30, "Website cannot exceed 30 characters"),

  email: z
    .string()
    .email("Please enter a valid email address"),

  phone: z
    .string()
    .min(10, "Phone number is too short")
    .max(20, "Phone number is too long"),
});

export type PublisherFormValues = z.infer<typeof publisherSchema>;