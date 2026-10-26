"use client"

import { useState } from "react"
import { useForm } from "react-hook-form"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { Eye, EyeOff, Loader2 } from "lucide-react"

import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"

import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Separator } from "@/components/ui/separator"

const registerSchema = z
  .object({
    name: z
      .string()
      .trim()
      .min(2, "Ad ən azı 2 simvol olmalıdır")
      .max(50, "Ad 50 simvoldan çox ola bilməz"),

    email: z
      .string()
      .trim()
      .min(1, "Email daxil edin")
      .email("Düzgün email daxil edin"),

    password: z
      .string()
      .min(1, "Şifrə daxil edin")
      .min(8, "Şifrə ən azı 8 simvol olmalıdır")
      .regex(/[A-Z]/, "Ən azı 1 böyük hərf olmalıdır")
      .regex(/[a-z]/, "Ən azı 1 kiçik hərf olmalıdır")
      .regex(/[0-9]/, "Ən azı 1 rəqəm olmalıdır"),

    confirmPassword: z
      .string()
      .min(1, "Şifrəni təsdiqləyin"),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Şifrələr eyni deyil",
    path: ["confirmPassword"],
  })

type RegisterFormValues = z.infer<typeof registerSchema>

export default function RegisterPage() {
  const [showPassword, setShowPassword] = useState(false)
  const [showConfirmPassword, setShowConfirmPassword] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

  const form = useForm<RegisterFormValues>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      name: "",
      email: "",
      password: "",
      confirmPassword: "",
    },
  })

  const onSubmit = async (values: RegisterFormValues) => {
    try {
      setIsLoading(true)

      const { confirmPassword, ...data } = values

      console.log(data)

      // API request
      // await register(data)

    } catch (error) {
      console.error(error)
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <main className="flex min-h-screen items-center justify-center bg-muted/40 px-4 py-8">
      <Card className="w-full max-w-md shadow-sm">
        <CardHeader className="flex flex-col items-center justify-center space-y-1">
          <CardTitle className="text-2xl font-semibold">
            Create an account
          </CardTitle>

          <CardDescription>
            Please enter your information to create a new account.
          </CardDescription>
        </CardHeader>

        <Separator />

        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmit)}
            className="space-y-6"
          >
            <CardContent className="pt-6">
              <div className="space-y-5">
                {/* Name */}
                <FormField
                  control={form.control}
                  name="name"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Name</FormLabel>

                      <FormControl>
                        <Input
                          {...field}
                          type="text"
                          placeholder="John Doe"
                          autoComplete="name"
                          disabled={isLoading}
                        />
                      </FormControl>

                      <FormMessage />
                    </FormItem>
                  )}
                />

                {/* Email */}
                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Email</FormLabel>

                      <FormControl>
                        <Input
                          {...field}
                          type="email"
                          placeholder="example@email.com"
                          autoComplete="email"
                          disabled={isLoading}
                        />
                      </FormControl>

                      <FormMessage />
                    </FormItem>
                  )}
                />

                {/* Password */}
                <FormField
                  control={form.control}
                  name="password"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Password</FormLabel>

                      <FormControl>
                        <div className="relative">
                          <Input
                            {...field}
                            type={showPassword ? "text" : "password"}
                            placeholder="••••••••"
                            autoComplete="new-password"
                            disabled={isLoading}
                            className="pr-10"
                          />

                          <button
                            type="button"
                            onClick={() =>
                              setShowPassword((prev) => !prev)
                            }
                            disabled={isLoading}
                            aria-label={
                              showPassword
                                ? "Hide password"
                                : "Show password"
                            }
                            className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground disabled:pointer-events-none disabled:opacity-50"
                          >
                            {showPassword ? (
                              <EyeOff className="h-4 w-4" />
                            ) : (
                              <Eye className="h-4 w-4" />
                            )}
                          </button>
                        </div>
                      </FormControl>

                      <FormMessage />
                    </FormItem>
                  )}
                />

                {/* Confirm Password */}
                <FormField
                  control={form.control}
                  name="confirmPassword"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Confirm password</FormLabel>

                      <FormControl>
                        <div className="relative">
                          <Input
                            {...field}
                            type={
                              showConfirmPassword
                                ? "text"
                                : "password"
                            }
                            placeholder="••••••••"
                            autoComplete="new-password"
                            disabled={isLoading}
                            className="pr-10"
                          />

                          <button
                            type="button"
                            onClick={() =>
                              setShowConfirmPassword(
                                (prev) => !prev
                              )
                            }
                            disabled={isLoading}
                            aria-label={
                              showConfirmPassword
                                ? "Hide password"
                                : "Show password"
                            }
                            className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground disabled:pointer-events-none disabled:opacity-50"
                          >
                            {showConfirmPassword ? (
                              <EyeOff className="h-4 w-4" />
                            ) : (
                              <Eye className="h-4 w-4" />
                            )}
                          </button>
                        </div>
                      </FormControl>

                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
            </CardContent>

            <Separator />

            <CardFooter className="flex flex-col gap-4">
              <Button
                type="submit"
                className="w-full"
                disabled={isLoading}
              >
                {isLoading && (
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                )}

                {isLoading ? "Creating account..." : "Create account"}
              </Button>

              <p className="text-center text-sm text-muted-foreground">
                Already have an coount?{" "}
                <a
                  href="/login"
                  className="font-medium text-foreground underline-offset-4 hover:underline"
                >
                  Sign in
                </a>
              </p>
            </CardFooter>
          </form>
        </Form>
      </Card>
    </main>
  )
}