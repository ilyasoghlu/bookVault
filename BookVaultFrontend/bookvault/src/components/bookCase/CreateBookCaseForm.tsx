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
  bookCaseSchema,
  BookCaseFormValues,
} from "@/types/bookCaseSchema";

import {
  createBookCase,
  updateBookCase,
} from "@/lib/bookCaseService";

import { BookCase } from "@/types/bookcase";
import z from "zod";

type Props = {
  mode: "create" | "edit";
  bookCase?: BookCase;
};

export default function CreateBookCaseForm({
  mode,
  bookCase,
}: Props) {
  const router = useRouter();

  const form = useForm<
  z.input<typeof bookCaseSchema>,
  unknown,
  z.output<typeof bookCaseSchema>
>({
  resolver: zodResolver(bookCaseSchema),
  defaultValues: {
    casenumber: bookCase?.casenumber ?? undefined,
    capacity: bookCase?.capacity ?? undefined,
    sectionid: bookCase?.sectionid ?? undefined,
  
  },
});

  const onSubmit = async (values: BookCaseFormValues) => {
    try {
      if (mode === "create") {
        await createBookCase(values);
      } else if (bookCase) {
        await updateBookCase(bookCase.id, values);
      }

      form.reset();
      router.push("/bookcases");
      router.refresh();
    } catch (error) {
      console.error("BookCase operation failed:", error);
    }
  };

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="space-y-5"
      >
        <FormField
          control={form.control}
          name="casenumber"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Case Number</FormLabel>
              <FormControl>
               <Input
                  type="number"
                  value={field.value ? String(field.value) : ""}
                  onChange={(e) => field.onChange(e.target.value)}
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
              <FormLabel>Capacity</FormLabel>
              <FormControl>
                <Input
                  type="number"
                  value={field.value ? String(field.value) : ""}
                  onChange={(e) => field.onChange(e.target.value)}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="sectionid"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Section Id</FormLabel>
              <FormControl>
                <Input
                  type="number"
                  value={field.value ? String(field.value) : ""}
                  onChange={(e) => field.onChange(e.target.value)}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />


        <Button type="submit" className="w-full">
          {mode === "create"
            ? "Create Book Case"
            : "Update Book Case"}
        </Button>
      </form>
    </Form>
  );
}