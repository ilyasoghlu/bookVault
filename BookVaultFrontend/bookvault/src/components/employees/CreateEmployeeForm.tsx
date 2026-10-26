"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRouter } from "next/navigation";

import {
  Form,
  FormField,
  FormItem,
  FormLabel,
  FormControl,
  FormMessage,
} from "@/components/ui/form";

import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

import {
  createEmployeeSchema,
  updateEmployeeSchema,
} from "@/types/employeeSchema";

import { createEmployee, updateEmployee } from "@/lib/employeeService";
import { Employee } from "@/types/employee";

type Props = {
  mode: "create" | "edit";
  employee?: Employee;
};



export default function CreateEmployeeForm({
  mode,
  employee,
}: Props) {
  const router = useRouter();

  type EmployeeFormData = {
  firstname: string;
  lastname: string;
  email: string;
  phone: string;
  employeeposition: string;
  hiredate: string;
  enddate?: string | null;
  username?: string;
  password?: string;
};

const form = useForm<EmployeeFormData>({
  resolver: zodResolver(
    mode === "create"
      ? createEmployeeSchema
      : updateEmployeeSchema
  ),
  defaultValues: {
    firstname: employee?.firstname ?? "",
    lastname: employee?.lastname ?? "",
    email: employee?.email ?? "",
    phone: employee?.phone ?? "",
    employeeposition: employee?.employeeposition ?? "",
    hiredate: employee?.hiredate ?? "",
    enddate: employee?.enddate ?? null,
    username: "",
    password: "",
  },
});

  const onSubmit = async (values: EmployeeFormData) => {
    try {
      if (mode === "create") {
        if (!values.username || !values.password) {
          return;
        }

        await createEmployee({
          firstname: values.firstname,
          lastname: values.lastname,
          email: values.email,
          phone: values.phone,
          username: values.username,
          password: values.password,
          employeeposition: values.employeeposition,
          hiredate: values.hiredate,
          enddate: values.enddate,
        });
      } else {
        await updateEmployee(employee!.id, {
          firstname: values.firstname,
          lastname: values.lastname,
          email: values.email,
          phone: values.phone,
          employeeposition: values.employeeposition,
          hiredate: values.hiredate,
          enddate: values.enddate,
        });
      }

      form.reset();
      router.push("/employees");
    } catch (error) {
      console.error("Employee error:", error);
    }
  };

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="space-y-4"
      >
        <FormField
          control={form.control}
          name="firstname"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Firstname</FormLabel>
              <FormControl>
                <Input {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="lastname"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Lastname</FormLabel>
              <FormControl>
                <Input {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="phone"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Phone</FormLabel>
              <FormControl>
                <Input {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="employeeposition"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Employee position</FormLabel>
              <FormControl>
                <Input {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        {mode === "create" && (
          <>
            <FormField
              control={form.control}
              name="username"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Username</FormLabel>
                  <FormControl>
                    <Input {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Password</FormLabel>
                  <FormControl>
                    <Input
                      type="password"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </>
        )}

        <FormField
          control={form.control}
          name="hiredate"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Hire date</FormLabel>
              <FormControl>
                <Input
                  type="date"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="enddate"
          render={({ field }) => (
            <FormItem>
              <FormLabel>End date</FormLabel>
              <FormControl>
                <Input
                  type="date"
                  value={field.value ?? ""}
                  onChange={(e) =>
                    field.onChange(
                      e.target.value || null
                    )
                  }
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <Button type="submit">
          {mode === "create"
            ? "Create Employee"
            : "Update Employee"}
        </Button>
      </form>
    </Form>
  );
}