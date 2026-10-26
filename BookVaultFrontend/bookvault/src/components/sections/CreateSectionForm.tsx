"use client";

import { useForm, Resolver } from "react-hook-form";
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
  createSectionSchema, 
  updateSectionSchema, 
  CreateSectionFormInput, 
  UpdateSectionFormInput, 
  CreateSectionFormValues,
  UpdateSectionFormValues
} 
  from "@/types/sectionSchema";

import { useEffect, useState } from "react";
import { getEmployees } from "@/lib/employeeService";
import { getRooms } from "@/lib/roomService";

import { createSection, updateSection } from "@/lib/sectionService";
import { Section } from "@/types/section";
import { Employee } from "@/types/employee";
import { Room } from "@/types/room";



type Props = {
  mode: "create" | "edit";
  section?: Section;
};


export default function CreateSectionForm({ mode, section }: Props) {
  const router = useRouter();
  const [employees, setEmployees] = useState<Employee[]>([])
  const [rooms, setRooms] = useState<Room[]>([])



  useEffect(() =>{
    getRooms()
      .then(setRooms)
      .catch((error) =>{
        console.error("Failed to load rooms:", error);
      })

    if(mode=== "edit"){
       getEmployees()
      .then(setEmployees)
      .catch((error) => {
        console.error("Failed to load employees:", error)
      });
    }
  }, [mode])


  const resolver: Resolver<
  CreateSectionFormInput | UpdateSectionFormInput,
  unknown,
  CreateSectionFormValues | UpdateSectionFormValues
> = zodResolver(
  mode === "create"
    ? createSectionSchema
    : updateSectionSchema
);

const form = useForm<
  CreateSectionFormInput | UpdateSectionFormInput,
  unknown,
  CreateSectionFormValues | UpdateSectionFormValues
>({
  resolver,
  defaultValues:
    mode === "create"
      ? {
          title: "",
          sectionnumber: 0,
          sectionfloor: 0,
          description: "",
          capacity: 0,
          roomid: 0,
        }
      : {
          title: section?.title ?? "",
          sectionnumber: section?.sectionnumber ?? 0,
          sectionfloor: section?.sectionfloor ?? 0,
          description: section?.description ?? "",
          capacity: section?.capacity ?? 0,
          employeeid: section?.employeeid ?? null,
          roomid: section?.roomid ?? 0,
        },
});


  const onSubmit = async (
  values: CreateSectionFormValues | UpdateSectionFormValues
) => {
  try {
    if (mode === "create") {
      await createSection({
        title: values.title,
        sectionnumber: values.sectionnumber,
        capacity: values.capacity,
        sectionfloor: values.sectionfloor,
        description: values.description,
        roomid: values.roomid,
      });
    } else {
      const updateValues = values as UpdateSectionFormValues;

      await updateSection(section!.id, {
        title: updateValues.title,
        sectionnumber: updateValues.sectionnumber,
        capacity: updateValues.capacity,
        sectionfloor: updateValues.sectionfloor,
        description: updateValues.description,
        roomid: updateValues.roomid,
        employeeid: updateValues.employeeid ?? null,
      });
    }

    form.reset();
    router.push("/sections");
  } catch (error) {
    console.error("Create section error:", error);
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
              <FormLabel>Section title</FormLabel>

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
            name="sectionnumber"
            render={({ field }) => (
                <FormItem>
                <FormLabel>Section number</FormLabel>

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

       <FormField
        control={form.control}
        name="sectionfloor"
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


      <FormField
        control={form.control}
        name="capacity"
        render={({ field }) => (
          <FormItem>
            <FormLabel>Section capacity</FormLabel>

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

      <FormField
        control={form.control}
        name="roomid"
        render={({ field }) => (
          <FormItem>
            <FormLabel>Room</FormLabel>

            <FormControl>
              <select
                {...field}
                value={field.value == null ? 0 : Number(field.value)}
                onChange={(e) => field.onChange(Number(e.target.value))}
                className="w-full rounded-md border px-3 py-2"
              >
                <option value={0}>Select room</option>

                {rooms.map((room) => (
                  <option key={room.id} value={room.id}>
                    {room.title} — Room {room.roomnumber}
                  </option>
                ))}
              </select>
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
                  {...field}
                  value={field.value == null ? 0 : Number(field.value)}
                  onChange={(e) =>
                    field.onChange(
                      e.target.value === "0"
                        ? null
                        : Number(e.target.value)
                    )
                  }
                  className="w-full rounded-md border px-3 py-2"
                >
                  <option value={0}>No employee assigned</option>

                  {employees.map((employee) => (
                    <option key={employee.id} value={employee.id}>
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
        {mode === "create" ? "Create Section" : "Update Section"}
      </Button>

      </form>
    </Form>
  );
}