import {z} from "zod";

export const createRoomSchema = z.object({
    title:z
    .string()
    .min(5, "Title must contain at least 5 characters")
    .max(30, "Title can't exceed 30 characters"),

    roomnumber:z.coerce
    .number()
    .int()
    .positive("Room number must be greater than 0"),

    capacity:z.coerce
    .number()
    .int()
    .positive("Capacity must be greater than 0"),

    roomfloor:z.coerce
    .number()
    .int()
    .nonnegative("Room floor can't be negative"),

    description:z
    .string()
    .min(10, "Description must contain at least 10 characters")
    .max(1000, "Description can't exceed 1000 characters"),
})
export const updateRoomSchema = z.object({
    title:z
    .string()
    .min(5, "Title must contain at least 5 characters")
    .max(30, "Title can't exceed 30 characters"),

    roomnumber:z.coerce
    .number()
    .int()
    .positive("Room number must be greater than 0"),

    capacity:z.coerce
    .number()
    .int()
    .positive("Capacity must be greater than 0"),

    description:z
    .string()
    .min(10, "Description must contain at least 10 characters")
    .max(1000, "Description can't exceed 1000 characters"),

    employeeid: z.coerce
    .number()
    .int()
    .positive("Employee ID must be greater than 0")
    .nullable()
    .optional(),
    })


export type CreateRoomFormInput = z.input<typeof createRoomSchema>;
export type CreateRoomFormValues = z.input<typeof createRoomSchema>;

export type UpdateRoomFormInput = z.output<typeof updateRoomSchema>;
export type UpdateRoomFormValues = z.output<typeof updateRoomSchema>;








