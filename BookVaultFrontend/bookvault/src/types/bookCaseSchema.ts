import { z } from "zod";

export const bookCaseSchema = z.object({
  casenumber: z.coerce
    .number()
    .int()
    .positive("Case number must be greater than 0"),

  capacity: z.coerce
    .number()
    .int()
    .positive("Capacity must be greater than 0"),

  sectionid: z.coerce
    .number()
    .int()
    .positive("Please select a section")
    .nullable()
    .optional(),

  roomid: z.coerce
    .number()
    .int()
    .positive("Please select a room")
    .nullable()
    .optional(),
});

export type BookCaseFormValues = z.infer<typeof bookCaseSchema>;