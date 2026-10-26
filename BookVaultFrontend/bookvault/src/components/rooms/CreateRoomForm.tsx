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
  createRoomSchema, 
  updateRoomSchema, 
  CreateRoomFormInput, 
  CreateRoomFormValues, 
  UpdateRoomFormInput, 
  UpdateRoomFormValues 
} from "@/types/roomSchema";

import { useEffect, useState } from "react";
import { getEmployees } from "@/lib/employeeService";
import { Employee } from "@/types/employee";

import { createRoom, updateRoom } from "@/lib/roomService";
import { Room } from "@/types/room";


type Props = {
  mode: "create" | "edit";
  room?: Room;
};


export default function CreateRoomForm({ mode, room }: Props) {
  const router = useRouter();
  const [employees, setEmployees] = useState<Employee[]>([]);

  useEffect(() =>{
  if(mode==="edit"){
    getEmployees()
      .then(setEmployees)
      .catch((error) => {
        console.error("Failed to load employees:", error)
      });
  }
},[mode]);

const schema = 
  mode === "create"
    ? createRoomSchema
    : updateRoomSchema;


  const form = useForm({
  resolver: zodResolver(
    mode === "create"
      ? createRoomSchema
      : updateRoomSchema
  ),
  defaultValues:
    mode === "create"
      ? {
          title: "",
          roomnumber: 0,
          roomfloor: 0,
          capacity: 0,
          description: "",
        }
      : {
          title: room?.title ?? "",
          roomnumber: room?.roomnumber ?? 0,
          capacity: room?.capacity ?? 0,
          description: room?.description ?? "",
          employeeid: room?.employeeid ?? null,
        },
});


  const onSubmit = async (values: any) => {
    try {
      if (mode === "create") {
        await createRoom({
          title: values.title,
          roomnumber: values.roomnumber,
          capacity: values.capacity,
          roomfloor: values.roomfloor,
          description: values.description,
      });
    } else if (room) {
        await updateRoom(room.id, {
          title: values.title,
          roomnumber: values.roomnumber,
          capacity: values.capacity,
          description: values.description,
          employeeid: values.employeeid ?? null,
      });
    }

    form.reset();
    router.push("/rooms");
    router.refresh();
    } catch (error) {
      console.error("Room operation failed:", error);
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
            name="title"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Room title</FormLabel>

                <FormControl>
                  <Input
                    type="text"
                    {...field}
                  />
                </FormControl>

                <FormMessage />
              </FormItem>
            )}
          />

       <FormField
            control={form.control}
            name="roomnumber"
            render={({ field }) => (
                <FormItem>
                <FormLabel>Room number</FormLabel>

                <FormControl>
                    <Input
                    type="number"
                    value={field.value as number}
                    onBlur={field.onBlur}
                    name={field.name}
                    ref={field.ref}
                    onChange={(e) =>
                        field.onChange(e.target.value)
                    }
                    />
                </FormControl>

                <FormMessage />
                </FormItem>
            )}
            />
      {mode=== "create" && (
        <FormField
        control={form.control}
        name="roomfloor"
        render={({ field }) => (
          <FormItem>
            <FormLabel>Floor</FormLabel>

            <FormControl>
              <Input
                type="number"
                value={field.value as number}
                onBlur={field.onBlur}
                name={field.name}
                ref={field.ref}
                onChange={(e) =>
                  field.onChange(e.target.value)
                }
                />
      </FormControl>

      <FormMessage />
    </FormItem>
  )}
/>
)}


    <FormField
      control={form.control}
      name="capacity"
      render={({ field }) => (
        <FormItem>
          <FormLabel>Room capacity</FormLabel>

          <FormControl>
            <Input
              type="number"
              value={field.value as number}
              onBlur={field.onBlur}
              name={field.name}
              ref={field.ref}
              onChange={(e) =>
                field.onChange(e.target.value)
              }
            />
          </FormControl>

      <FormMessage />
    </FormItem>
  )}
/>

{mode === "edit" && (
  <FormField
    control={form.control}
    name="employeeid"
    render={({ field }) => (
      <FormItem>
        <FormLabel>Employee</FormLabel>

        <FormControl>
          <select
            className="w-full rounded-md border px-3 py-2"
            value={field.value == null ? "" : String(field.value)}
            onChange={(e) => {
              const value = e.target.value;

              field.onChange(
                value === "" ? null : Number(value)
              );
            }}
          >
            <option value="">No employee</option>

            {employees.map((employee) => (
              <option
                key={employee.id}
                value={employee.id}
              >
                {employee.firstname} {employee.lastname}
              </option>
            ))}
          </select>
        </FormControl>

        <FormMessage />
      </FormItem>
    )}
  />
)}

        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Description</FormLabel>

              <FormControl>
                <Input
                  type="text"
                  {...field}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />


        <Button type="submit">
          {mode === "create" ? "Create Room" : "Update Room"}
        </Button>

      </form>
    </Form>
  );
}