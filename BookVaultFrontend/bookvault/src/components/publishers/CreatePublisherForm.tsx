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



import { publisherSchema, PublisherFormValues } from "@/types/publisherSchema";
import { createPublisher, updatePublisher } from "@/lib/publisherService";
import { Publisher } from "@/types/publisher";




type Props = {
  mode:"create" | "edit" ;
  publisher?: Publisher; 
  };


export default function CreatePublisherForm({mode, publisher}:Props) {
  const router = useRouter();
  

  const form = useForm<PublisherFormValues>({
    resolver: zodResolver(publisherSchema),
      defaultValues: {
        publishername: publisher?.publishername ?? "",
        address: publisher?.address ?? "",
        city: publisher?.city ?? "",
        website: publisher?.website ?? "",
        email: publisher?.email ?? "",
        phone: publisher?.phone ?? "",
  }
  });


  const onSubmit = async (values: PublisherFormValues) => {
      try {
        const payload = {
          ...values,
        };

        if(mode ==="create"){
          await createPublisher(payload);

        }else {
          await updatePublisher(publisher!.id, payload);
        }  
        form.reset();
        router.push("/publishers");

      } catch (error) {
        console.error("Create publisher error:", error);
      }
    };


  return (
    <Form {...form}>
      <form
         onSubmit={form.handleSubmit(
            onSubmit,
            (errors) => {
              console.log("PUBLISHER FORM ERRORS:", errors);
            }
          )}
        className="space-y-4"
      >

        <FormField
          control={form.control}
          name="publishername"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Publisher name</FormLabel>

              <FormControl>
                <Input {...field} />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />


        <FormField
          control={form.control}
          name="address"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Address</FormLabel>

              <FormControl>
                <Input {...field} />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />


        <FormField
          control={form.control}
          name="city"
          render={({ field }) => (
            <FormItem>
              <FormLabel>City</FormLabel>

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
                    <Input type="email"
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
          name="website"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Website</FormLabel>

              <FormControl>
                <Input {...field} />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />


        <Button type="submit" >
            {mode === "create"? "Create Publisher" : "Update Publisher"}
        </Button>
      </form>
    </Form>
  );
}