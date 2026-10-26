
import {z} from "zod";

export const createShelfSchema = z.object({
    shelvecode: z.coerce
        .number()
        .int()
        .positive("Shelve code  must be greater than 0"),

    capacity:z.coerce
        .number()
        .int()
        .positive("Capacity must be greater than 0"),

    sectionid: z.coerce
        .number()
        .int()
        .positive()
        .optional(),

    caseid: z.coerce
        .number()
        .int()
        .positive("Book Case must be selected!"),



})
export const updateShelfSchema = z.object({
    shelvecode: z.coerce
        .number()
        .int()
        .positive("Shelve code  must be greater than 0"),

    capacity:z.coerce
        .number()
        .int()
        .positive("Capacity must be greater than 0"),

    sectionid: z.coerce
        .number()
        .int()
        .positive()
        .optional(),

   

})

export type CreateShelfFormInput = z.input<typeof createShelfSchema>;
export type CreateShelfFormValues = z.output<typeof createShelfSchema>;

export type UpdateShelfFormInput = z.input<typeof updateShelfSchema>;
export type UpdateShelfFormValues = z.output<typeof updateShelfSchema>;