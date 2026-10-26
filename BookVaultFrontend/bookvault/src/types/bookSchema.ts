import { z } from "zod";

export const bookSchema = z.object({
   isbn: z
    .string()
    .min(1, "ISBN is required")
    .max(36, "ISBN can't be greater than 36 symbols"),

  title: z
    .string()
    .min(1, "Book title is required"),

  category: z
    .string()
    .min(1, "Category is required"),

  genre: z
    .string()
    .min(1, "Genre is required"),

  publicationdate: z
    .string()
    .min(1, "Publication date is required"),

  pagecount: z
    .number()
    .min(1, "Page count must be greater than 0"),

  edition: z
    .string()
    .min(1, "Edition is required"),

  totalcopies: z
    .number()
    .min(1, "Total copies must be greater than 0"),

  language: z
    .string()
    .min(1, "Language is required"),

  authorid: z
    .string()
    .min(1, "Author is required"),

  publisherid: z
    .string()
    .min(1, "Publisher is required"),

  shelfid: z
    .number()
    .min(1, "Shelf is required"),
});

export type BookFormInput = z.input<typeof bookSchema>;

export type BookFormValues = z.output<typeof bookSchema>;