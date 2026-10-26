import { z } from "zod";

const employeeFields = {
    firstname: z
        .string()
        .min(2, "Firstname must contain at least 2 characters")
        .max(50, "Firstname can't exceed 50 characters"),

    lastname: z
        .string()
        .min(2, "Lastname must contain at least 2 characters")
        .max(50, "Lastname can't exceed 50 characters"),

    email: z
        .string()
        .email("Please enter a valid email address"),

    phone: z
        .string()
        .min(10, "Phone number is too short")
        .max(20, "Phone number is too long"),

    employeeposition: z
        .string()
        .min(2, "Employee position must contain at least 2 characters")
        .max(100, "Employee position can't exceed 100 characters"),

    hiredate: z
        .string()
        .min(1, "Hire date is required"),

    enddate: z
        .string()
        .nullable()
        .optional(),
};

export const createEmployeeSchema = z.object({
    ...employeeFields,

    username: z
        .string()
        .min(2, "Username must contain at least 2 characters")
        .max(30, "Username can't exceed 30 characters"),

    password: z
        .string()
        .min(6, "Password must contain at least 6 characters"),
});

export const updateEmployeeSchema = z.object({
    ...employeeFields,
});

export type CreateEmployeeFormValues =
    z.infer<typeof createEmployeeSchema>;

export type UpdateEmployeeFormValues =
    z.infer<typeof updateEmployeeSchema>;