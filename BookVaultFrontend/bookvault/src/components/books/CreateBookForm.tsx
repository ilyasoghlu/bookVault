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
  bookSchema,
  BookFormInput,
  BookFormValues,
} from "@/types/bookSchema";

import {
  createBook,
  updateBook,
} from "@/lib/bookService";

import { Book } from "@/types/book";

import { getAuthors } from "@/lib/authorService";
import { getPublishers } from "@/lib/publisherService";
import { getShelves } from "@/lib/shelfService";

import { Author } from "@/types/author";
import { Publisher } from "@/types/publisher";
import { Shelf } from "@/types/shelf";

type Props = {
  mode: "create" | "edit";
  book?: Book;
};

export default function CreateBookForm({
  mode,
  book,
}: Props) {
  const router = useRouter();

  const [authors, setAuthors] = useState<Author[]>([]);
  const [publishers, setPublishers] = useState<Publisher[]>([]);
  const [shelves, setShelves] = useState<Shelf[]>([]);

  const [loadingOptions, setLoadingOptions] = useState(true);

  const form = useForm<BookFormInput, unknown, BookFormValues>({
    resolver: zodResolver(bookSchema),

    defaultValues: {
      isbn: book?.isbn ?? "",
      title: book?.title ?? "",
      category: book?.category ?? "",
      genre: book?.genre ?? "",
      publicationdate: book?.publicationdate ?? "",
      pagecount: book?.pagecount ?? 0,
      edition: book?.edition ?? "",
      totalcopies: book?.totalcopies ?? 0,
      language: book?.language ?? "",

      authorid: book?.authorid ?? "",
      publisherid: book?.publisherid ?? "",
      shelfid: book?.shelfid ?? 0,
    },
  });

  useEffect(() => {
    const loadOptions = async () => {
      try {
        const [
          authorsData,
          publishersData,
          shelvesData,
        ] = await Promise.all([
          getAuthors(),
          getPublishers(),
          getShelves(),
        ]);

        setAuthors(authorsData);
        setPublishers(publishersData);
        setShelves(shelvesData);
      } catch (error) {
        console.error(
          "Failed to load book options:",
          error
        );
      } finally {
        setLoadingOptions(false);
      }
    };

    loadOptions();
  }, []);

  const onSubmit = async (values: BookFormValues) => {
    try {
      console.log("BOOK DATA:", values);

      if (mode === "create") {
        await createBook(values);
      } else {
        if (!book?.id) {
          throw new Error("Book ID is missing");
        }

        await updateBook(book.id, values);
      }

      form.reset();

      router.push("/books");
    } catch (error: unknown) {
      console.error("Book error:", error);

      if (
        typeof error === "object" &&
        error !== null &&
        "response" in error
      ) {
        const axiosError = error as {
          response?: {
            data?: unknown;
          };
        };

        console.error(
          "Backend response:",
          axiosError.response?.data
        );
      }
    }
  };

  if (loadingOptions) {
    return <div>Loading...</div>;
  }

  return (
    <Form {...form}>
     
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="space-y-2 grid grid-cols-1 md:grid-cols-2 gap-1"
      >
        {/* ISBN */}

        <FormField
          control={form.control}
          name="isbn"
          render={({ field }) => (
            <FormItem>
              <FormLabel>ISBN</FormLabel>

              <FormControl>
                <Input
                  type="text"
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                  {...field}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* TITLE */}

        <FormField
          control={form.control}
          name="title"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Book title</FormLabel>

              <FormControl>
                <Input  
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                  type="text"
                  {...field}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* AUTHOR */}

        <FormField
          control={form.control}
          name="authorid"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Author</FormLabel>

              <FormControl>
                <select
                  {...field}
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2" 
                >
                  <option value="">
                    Select author
                  </option>

                  {authors.map((author) => (
                    <option
                      key={author.id}
                      value={author.id}
                    >
                      {author.firstname}{" "}
                      {author.lastname}
                    </option>
                  ))}
                </select>
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* PUBLISHER */}

        <FormField
          control={form.control}
          name="publisherid"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Publisher</FormLabel>

              <FormControl>
                <select
                  {...field}
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                >
                  <option value="">
                    Select publisher
                  </option>

                  {publishers.map((publisher) => (
                    <option
                      key={publisher.id}
                      value={publisher.id}
                    >
                      {publisher.publishername}
                    </option>
                  ))}
                </select>
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* SHELF */}

        <FormField
          control={form.control}
          name="shelfid"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Shelf</FormLabel>

              <FormControl>
                <select
                  value={field.value}
                  onChange={(e) =>
                    field.onChange(
                      Number(e.target.value)
                    )
                  }
                  onBlur={field.onBlur}
                  name={field.name}
                  ref={field.ref}
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                >
                  <option value={0}>
                    Select shelf
                  </option>

                  {shelves.map((shelf) => (
                    <option
                      key={shelf.id}
                      value={shelf.id}
                    >
                      {shelf.shelvecode}
                    </option>
                  ))}
                </select>
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* CATEGORY */}

        <FormField
          control={form.control}
          name="category"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Category</FormLabel>

              <FormControl>
                <Input
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                  type="text"
                  {...field}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* GENRE */}

        <FormField
          control={form.control}
          name="genre"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Genre</FormLabel>

              <FormControl>
                <Input
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                  type="text"
                  {...field}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* PUBLICATION DATE */}

        <FormField
          control={form.control}
          name="publicationdate"
          render={({ field }) => (
            <FormItem>
              <FormLabel>
                Publication date
              </FormLabel>

              <FormControl>
                <Input
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                  type="date"
                  {...field}
                  value={field.value ?? ""}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* PAGE COUNT */}

        <FormField
          control={form.control}
          name="pagecount"
          render={({ field }) => (
            <FormItem>
              <FormLabel>
                Page count
              </FormLabel>

              <FormControl>
                <Input
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                  type="number"
                  value={field.value ?? ""}
                  onChange={(e) =>
                    field.onChange(
                      e.target.value === ""
                        ? 0
                        : Number(e.target.value)
                    )
                  }
                  onBlur={field.onBlur}
                  name={field.name}
                  ref={field.ref}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* EDITION */}

        <FormField
          control={form.control}
          name="edition"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Edition</FormLabel>

              <FormControl>
                <Input
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                  type="text"
                  {...field}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* TOTAL COPIES */}

        <FormField
          control={form.control}
          name="totalcopies"
          render={({ field }) => (
            <FormItem>
              <FormLabel>
                Total copies
              </FormLabel>

              <FormControl>
                <Input
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2"
                  type="number"
                  value={field.value ?? ""}
                  onChange={(e) =>
                    field.onChange(
                      e.target.value === ""
                        ? 0
                        : Number(e.target.value)
                    )
                  }
                  onBlur={field.onBlur}
                  name={field.name}
                  ref={field.ref}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* LANGUAGE */}

        <FormField
          control={form.control}
          name="language"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Language</FormLabel>

              <FormControl>
                <Input
                  className="w-full rounded-md border hover:border-blue-400  px-3 py-2" 
                  type="text"
                  {...field}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        {/* SUBMIT */}

        <Button
          type="submit"
          disabled={form.formState.isSubmitting}
        >
          {form.formState.isSubmitting
            ? "Saving..."
            : mode === "create"
              ? "Create Book"
              : "Update Book"}
        </Button>
      </form>
    </Form>
  );
}