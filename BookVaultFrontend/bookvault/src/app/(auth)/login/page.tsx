"use client"

import { useState } from "react"
import { useForm } from "react-hook-form"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"

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

import { Eye, EyeOff, Loader2 } from "lucide-react"

import { useRouter } from "next/navigation"
import { useAuth } from "@/hooks/useAuth"


const loginSchema = z.object({
  email: z
    .string()
    .min(1, "Please, enter your email")
    .email("Please, enter a valid email"),

  password: z
    .string()
    .min(1, "Please enter your password")
    .min(6, "Password must be minimum 6 symbols"),
})

type LoginFormValues = z.infer<typeof loginSchema>

export default function LoginPage() {
  const [showPassword, setShowPassword] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

  const router = useRouter();
  const {login} = useAuth()

  const form = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  })

  const onSubmit = async (values: LoginFormValues) => {
  try {
    setIsLoading(true)

    await login(values.email, values.password)

    router.replace("/dashboard")
  } catch (error) {
    console.error(error)
  } finally {
    setIsLoading(false)
  }
}

  return (
    <main className="flex min-h-screen items-center justify-center bg-muted/40 px-4">
      <Card className="w-full max-w-md shadow-sm ">
        <CardHeader className="flex flex-col items-center justify-center space-y-1">
          <CardTitle className="text-2xl font-semibold">
            Welcome back
          </CardTitle>

          <CardDescription>
            Enter your credentials to sign in to your account.
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
                      <div className="flex items-center justify-between">
                        <FormLabel>Password</FormLabel>

                        <a
                          href="/forgot-password"
                          className="text-sm text-muted-foreground transition-colors hover:text-primary "
                        >
                          Forgot password?
                        </a>
                      </div>

                      <FormControl>
                        <div className="relative">
                          <Input
                            {...field}
                            type={showPassword ? "text" : "password"}
                            placeholder="••••••••"
                            autoComplete="current-password"
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
                            className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground transition-colors hover:text-foreground disabled:pointer-events-none disabled:opacity-50"
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
              </div>
            </CardContent>

            <CardFooter className="flex flex-col gap-4">
              <Button
                type="submit"
                className="w-full bg-blue-400"
                disabled={isLoading}
              >
                {isLoading && (
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                )}

                {isLoading ? "Signing in..." : "Sign in"}
              </Button>

              <p className="text-center text-sm text-muted-foreground shimmer-color-blue-700">
                Don’t have an account yet? {" "}
                <a
                  href="/register"
                  className="font-medium text-foreground underline-offset-4 hover:underline "
                >
                  Sign up
                </a>
              </p>
            </CardFooter>
          </form>
        </Form>
      </Card>
    </main>
  )
}