"use client";

import { useEffect, useState } from "react";

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
  createBorrowingSchema,
  updateBorrowingSchema,
  CreateBorrowingFormInput,
  CreateBorrowingFormValues,
  UpdateBorrowingFormInput,
  UpdateBorrowingFormValues,
} from "@/types/borrowingSchema";

import {
  createBorrowing,
  updateBorrowing,
} from "@/lib/borrowingService";

import { Borrowing } from "@/types/borrowing";

import { getBooks } from "@/lib/bookService";
import { getReaders } from "@/lib/readerService";

import { Book } from "@/types/book";
import { Reader } from "@/types/reader";

type Props = {
  mode: "create" | "edit";
  borrowing?: Borrowing;
};

export default function CreateBorrowingForm({
  mode,
  borrowing,
}: Props) {
  const router = useRouter();

  const [books, setBooks] = useState<Book[]>([]);
  const [readers, setReaders] = useState<Reader[]>([]);
  const [loadingOptions, setLoadingOptions] = useState(
    mode === "create"
  );

  const createForm = useForm<
    CreateBorrowingFormInput,
    unknown,
    CreateBorrowingFormValues
  >({
    resolver: zodResolver(createBorrowingSchema),
    defaultValues: {
      duedate: borrowing?.duedate ?? "",
      readerid: borrowing?.readerid ?? 0,
      bookid: borrowing?.bookid ?? "",
    },
  });

  const updateForm = useForm<
    UpdateBorrowingFormInput,
    unknown,
    UpdateBorrowingFormValues
  >({
    resolver: zodResolver(updateBorrowingSchema),
    defaultValues: {
      duedate: borrowing?.duedate ?? "",
      returndate: borrowing?.returndate ?? "",
    },
  });

  useEffect(() => {
    if (mode !== "create") {
      return;
    }

    const loadOptions = async () => {
      try {
        const [bookData, readerData] = await Promise.all([
          getBooks(),
          getReaders(),
        ]);

        setBooks(bookData);
        setReaders(readerData);
      } catch (error) {
        console.error(
          "Failed to load book and reader options:",
          error
        );
      } finally {
        setLoadingOptions(false);
      }
    };

    loadOptions();
  }, [mode]);

  const handleCreate = async (
    values: CreateBorrowingFormValues
  ) => {
    try {
      await createBorrowing(values);

      router.push("/borrowings");
      router.refresh();
    } catch (error) {
      console.error(
        "Failed to create borrowing:",
        error
      );
    }
  };

  const handleUpdate = async (
    values: UpdateBorrowingFormValues
  ) => {
    try {
      if (!borrowing?.id) {
        throw new Error("Borrowing ID is missing");
      }

      await updateBorrowing(borrowing.id, values);

      router.push("/borrowings");
      router.refresh();
    } catch (error) {
      console.error(
        "Failed to update borrowing:",
        error
      );
    }
  };

  if (mode === "create" && loadingOptions) {
    return <div>Loading...</div>;
  }

  if (mode === "create") {
    return (
      <Form {...createForm}>
        <h2>Create Borrowing</h2>
        <form
          onSubmit={createForm.handleSubmit(handleCreate)}
          className="space-y-4"
        >
          {/* BOOK */}

          <FormField
            control={createForm.control}
            name="bookid"
            render={({ field }) => (
              <FormItem>
                <FormLabel>
                  Book
                </FormLabel>

                <FormControl>
                  <select
                    value={field.value ?? ""}
                    onChange={(event) => {
                      field.onChange(
                        event.target.value
                      );
                    }}
                    onBlur={field.onBlur}
                    name={field.name}
                    ref={field.ref}
                    className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                  >
                    <option value="">
                      Select a book
                    </option>

                    {books.map((book) => (
                      <option
                        key={book.id}
                        value={book.id}
                      >
                        {book.title}
                      </option>
                    ))}
                  </select>
                </FormControl>

                <FormMessage />
              </FormItem>
            )}
          />

          {/* READER */}

          <FormField
            control={createForm.control}
            name="readerid"
            render={({ field }) => (
              <FormItem>
                <FormLabel>
                  Reader
                </FormLabel>

                <FormControl>
                  <select
                    value={String(
                      field.value ?? 0
                    )}
                    onChange={(event) => {
                      field.onChange(
                        Number(
                          event.target.value
                        )
                      );
                    }}
                    onBlur={field.onBlur}
                    name={field.name}
                    ref={field.ref}
                    className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                  >
                    <option value="0">
                      Select a reader
                    </option>

                    {readers.map((reader) => (
                      <option
                        key={reader.id}
                        value={String(
                          reader.id
                        )}
                      >
                        {reader.firstname}{" "}
                        {reader.lastname}
                      </option>
                    ))}
                  </select>
                </FormControl>

                <FormMessage />
              </FormItem>
            )}
          />

          {/* DUE DATE */}

          <FormField
            control={createForm.control}
            name="duedate"
            render={({ field }) => (
              <FormItem>
                <FormLabel>
                  Due date
                </FormLabel>

                <FormControl>
                  <Input
                    type="date"
                    value={field.value ?? ""}
                    onChange={field.onChange}
                    onBlur={field.onBlur}
                    name={field.name}
                    ref={field.ref}
                  />
                </FormControl>

                <FormMessage />
              </FormItem>
            )}
          />

          {/* SUBMIT */}

          <Button
            type="submit"
            disabled={
              createForm.formState.isSubmitting
            }
          >
            {createForm.formState.isSubmitting
              ? "Saving..."
              : "Create Borrowing"}
          </Button>
        </form>
      </Form>
    );
  }

  return (
    <Form {...updateForm}>
      <form
        onSubmit={updateForm.handleSubmit(handleUpdate)}
        className="space-y-4"
      >
        {/* DUE DATE */}

        <FormField
          control={updateForm.control}
          name="duedate"
          render={({ field }) => (
            <FormItem>
              <FormLabel>
                Due date
              </FormLabel>

              <FormControl>
                <Input
                  type="date"
                  value={field.value ?? ""}
                  onChange={field.onChange}
                  onBlur={field.onBlur}
                  name={field.name}
                  ref={field.ref}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* RETURN DATE */}

        <FormField
          control={updateForm.control}
          name="returndate"
          render={({ field }) => (
            <FormItem>
              <FormLabel>
                Return date
              </FormLabel>

              <FormControl>
                <Input
                  type="date"
                  value={field.value ?? ""}
                  onChange={field.onChange}
                  onBlur={field.onBlur}
                  name={field.name}
                  ref={field.ref}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* SUBMIT */}

        <Button
          type="submit"
          disabled={                    
            borrowing?.returndate !== null 
            ? true
            : updateForm.formState.isSubmitting
          }
        >
          {updateForm.formState.isSubmitting
            ? "Saving..."
            : borrowing?.returndate !== null
            ? "Returned"
            : "Update Borrowing"}
        </Button>
      </form>
    </Form>
  );
}