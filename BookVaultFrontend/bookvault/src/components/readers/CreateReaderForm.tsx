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

import { readerSchema, ReaderFormValues } from "@/types/readerSchema";
import { createReader, updateReader } from "@/lib/readerService";
import { Reader } from "@/types/reader";

type Props = {
  mode: "create" | "edit";
  reader?: Reader;
};

export default function CreateReaderForm({ mode, reader }: Props) {
  const router = useRouter();

  const form = useForm<ReaderFormValues>({
    resolver: zodResolver(readerSchema),
    defaultValues: {
      firstname: reader?.firstname ?? "",
      lastname: reader?.lastname ?? "",
      email: reader?.email ?? "",
      phone: reader?.phone ?? "",
      username: "",
      password: "",
      dateofbirth: reader?.dateofbirth ?? "",
      expirationdate: reader?.expirationdate ?? "",
    },
  });

 const onSubmit = async (values: ReaderFormValues) => {
  try {
    if (mode === "create") {
      if (!values.username || values.username.trim().length < 2) {
        form.setError("username", {
          message: "Username must contain at least 2 characters",
        });
        return;
      }

      if (!values.password || values.password.length < 8) {
        form.setError("password", {
          message: "Password must contain at least 8 characters",
        });
        return;
      }

      await createReader({
        firstname: values.firstname,
        lastname: values.lastname,
        email: values.email,
        phone: values.phone,
        username: values.username,
        password: values.password,
        dateofbirth: values.dateofbirth,
        expirationdate: values.expirationdate,
      });
    } else {
      await updateReader(reader!.id, {
        firstname: values.firstname,
        lastname: values.lastname,
        email: values.email,
        phone: values.phone,
        dateofbirth: values.dateofbirth,
        expirationdate: values.expirationdate,
      });
    }

    form.reset();
    router.push("/readers");
  } catch (error) {
    console.error("Reader error:", error);
  }
};

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className={`${mode === "create" ? "grid grid-cols-1 md:grid-cols-2 gap-2" : "space-y-4"}`}
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
                <Input type="email" {...field} />
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
                    <Input type="password" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </>
        )}


        <FormField
          control={form.control}
          name="expirationdate"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Membership expiration date</FormLabel>
              <FormControl>
                <Input type="date" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />


        <FormField
          control={form.control}
          name="dateofbirth"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Birthday</FormLabel>
              <FormControl>
                <Input type="date" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <Button type="submit">
          {mode === "create" ? "Create Reader" : "Update Reader"}
        </Button>
      </form>
    </Form>
  );
}