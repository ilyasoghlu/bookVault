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
import { Textarea } from "../ui/textarea"; 
import { Button } from "@/components/ui/button";

import {
  authorSchema,
  AuthorFormValues,
} from "@/types/authorSchema";

import { Author } from "@/types/author";

import { createAuthor, updateAuthor } from "@/lib/authorService";

import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { useEffect, useState } from "react";
import { Publisher } from "@/types/publisher";
import { getPublishers } from "@/lib/publisherService";

type Props = {
  mode:"create" | "edit" ;
  author?: Author; 
  };


export default function CreateAuthorForm({mode, author}:Props) {

  const router = useRouter();
  const [publishers, setPublishers] = useState<Publisher[]>([]); 

  useEffect(() => {
  const loadPublishers = async () => {
    try {
      const data = await getPublishers();
      setPublishers(data);
    } catch (error) {
      console.error("Failed to load publishers:", error);
    }
  };

  loadPublishers();
}, []);

  const form = useForm<AuthorFormValues>({
    resolver: zodResolver(authorSchema),
      defaultValues: {
      firstname: author?.firstname ?? "",
      lastname: author?.lastname ?? "",
      birthdate: author?.birthdate ?? "",
      deathdate: author?.deathdate ?? "",
      nationality: author?.nationality ?? "",
      biography: author?.biography ?? "",
      publisherid: author?.publisherid?? null
  }
  });


  const onSubmit = async (values: AuthorFormValues) => {
  try {
    const payload = {
      ...values,
      deathdate: values.deathdate || null,
      publisherid: values.publisherid || null,
    };

    

    if(mode ==="create"){
       await createAuthor(payload);

    }else {
      await updateAuthor(author!.id, payload);
    }
    
    form.reset();
    router.push("/authors");

  } catch (error) {
    console.error("Create author error:", error);
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
          name="birthdate"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Birthdate</FormLabel>

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
            name="deathdate"
            render={({ field }) => (
                <FormItem>
                <FormLabel>Deathdate</FormLabel>

                <FormControl>
                    <Input type="date"
                    {...field}
                    value={field.value ?? ""}
                    />
                </FormControl>

                <FormMessage />
                </FormItem>
            )}
            />

        <FormField
            control={form.control}
            name="publisherid"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Publisher</FormLabel>

                <Select
                  value={field.value ?? "none"}
                  onValueChange={(value) =>
                    field.onChange(value === "none" ? null : value)
                  }
                >
                  <FormControl>
                    <SelectTrigger>
                      <SelectValue placeholder="Select publisher" />
                    </SelectTrigger>
                  </FormControl>

                  <SelectContent>
                    <SelectItem value="none">
                      No publisher
                    </SelectItem>

                    {publishers.map((publisher) => (
                      <SelectItem
                        key={publisher.id}
                        value={publisher.id}
                      >
                        {publisher.publishername}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>

                <FormMessage />
              </FormItem>
            )}
          />    

        <FormField
          control={form.control}
          name="nationality"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Nationality</FormLabel>

              <FormControl>
                <Input {...field} />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />


        <FormField
          control={form.control}
          name="biography"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Biography</FormLabel>

              <FormControl>
                <Textarea {...field} />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />


        <Button type="submit" >
            {mode === "create"? "Create Author" : "Update Author"}
        </Button>
      </form>
    </Form>
  );
}