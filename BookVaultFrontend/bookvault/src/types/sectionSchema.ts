import {z} from "zod";

export const createSectionSchema = z.object({
    title:z
    .string()
    .min(2, "Title must contain at least 2 characters")
    .max(30, "Title can't exceed 30 characters"),

    sectionnumber:z.coerce
    .number()
    .int()
    .positive("Section number must be greater than 0"),

    capacity:z.coerce
    .number()
    .int()
    .nonnegative("Capacity cannot be negative"),

    sectionfloor:z.coerce
    .number()
    .int()
    .nonnegative("Floor can't be negative"),

    
    description:z
    .string()
    // .optional()
    .min(10, "Description must contain at least 10 characters")
    .max(1000, "Please enter less than 1000 symbol"),
    
    
    roomid: z.coerce
    .number()
    .int()
    .positive("Room ID must be greater than 0"),

})
export const updateSectionSchema = z.object({
    title:z
    .string()
    .min(2, "Title must contain at least 2 characters")
    .max(30, "Title can't exceed 30 characters"),

    sectionnumber:z.coerce
    .number()
    .int()
    .positive("Section number must be greater than 0"),

    capacity:z.coerce
    .number()
    .int()
    .nonnegative("Capacity cannot be negative"),

    sectionfloor:z.coerce
    .number()
    .int()
    .nonnegative("Floor can't be negative"),

    
    description:z
    .string()
    // .optional()
    .min(10, "Description must contain at least 10 characters")
    .max(1000, "Please enter less than 1000 symbol"),
    
     employeeid: z.coerce
        .number()
        .int()
        .positive("Employee ID must be greater than 0")
        .nullable()
        .optional(),
    
    roomid: z.coerce
    .number()
    .int()
    .positive("Room ID must be greater than 0"),

})

export type CreateSectionFormInput = z.input<typeof createSectionSchema>
export type UpdateSectionFormInput = z.input<typeof updateSectionSchema>
export type CreateSectionFormValues = z.output<typeof createSectionSchema>
export type UpdateSectionFormValues = z.output<typeof updateSectionSchema>