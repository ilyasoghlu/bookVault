"use client";

import { useEffect, useState } from "react";
import { useForm, type Resolver } from "react-hook-form";
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
createShelfSchema,
updateShelfSchema,
} from "@/types/shelfSchema";

import { createShelf, updateShelf } from "@/lib/shelfService";
import { Shelf } from "@/types/shelf";

import { getBookCase } from "@/lib/bookCaseService";
import { BookCase } from "@/types/bookcase";

type Props = {
mode: "create" | "edit";
shelf?: Shelf;
};

type ShelfFormValues = {
shelvecode: number;
capacity: number;
caseid?: number;
};

export default function CreateShelfForm({ mode, shelf }: Props) {
const router = useRouter();

const [bookCases, setBookCases] = useState<BookCase[]>([]);

useEffect(() => {
const fetchBookCase = async () => {
try {
const data = await getBookCase();
setBookCases(data);
} catch (error) {
console.error("Error fetching book cases:", error);
}
};

fetchBookCase();

}, []);

const form = useForm<ShelfFormValues>({
resolver: zodResolver(
mode === "create"
? createShelfSchema
: updateShelfSchema
) as Resolver<ShelfFormValues>,

defaultValues: {
  shelvecode: shelf?.shelvecode ?? 0,
  capacity: shelf?.capacity ?? 0,
  ...(mode === "create"
    ? {
        caseid: shelf?.caseid ?? 0,
      }
    : {}),
},

});

const onSubmit = async (values: ShelfFormValues) => {
try {
if (mode === "create") {
if (values.caseid === undefined) {
return;
}

    await createShelf({
      shelvecode: values.shelvecode,
      capacity: values.capacity,
      caseid: values.caseid,
    });
  } else {
    if (!shelf) {
      return;
    }

    await updateShelf(shelf.id, {
      shelvecode: values.shelvecode,
      capacity: values.capacity,
    });
  }

  form.reset();
  router.push("/shelves");
} catch (error) {
  console.error("Shelf save error:", error);
}

};

return (
<Form {...form}>
<form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4" >
<FormField
control={form.control}
name="shelvecode"
render={({ field }) => (
<FormItem>
<FormLabel>Shelf code</FormLabel>

          <FormControl>
            <Input
              type="number"
              value={field.value}
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
          <FormLabel>Shelf capacity</FormLabel>

          <FormControl>
            <Input
              type="number"
              value={field.value}
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

    {mode === "create" && (
      <FormField
        control={form.control}
        name="caseid"
        render={({ field }) => (
          <FormItem>
            <FormLabel>Book Case</FormLabel>

            <FormControl>
              <select
                className="border rounded-md p-2 w-full"
                value={String(field.value ?? "")}
                onChange={(e) =>
                  field.onChange(e.target.value)
                }
                onBlur={field.onBlur}
                name={field.name}
                ref={field.ref}
              >
                <option value="">
                  Select Book Case
                </option>

                {bookCases.map((bookCase) => (
                  <option
                    key={bookCase.id}
                    value={bookCase.id}
                  >
                    Case {bookCase.casenumber}
                  </option>
                ))}
              </select>
            </FormControl>

            <FormMessage />
          </FormItem>
        )}
      />
    )}

    <Button type="submit">
      {mode === "create"
        ? "Create Shelf"
        : "Update Shelf"}
    </Button>
  </form>
</Form>

);
}