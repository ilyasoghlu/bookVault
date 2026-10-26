"use client"

import { Suspense, useEffect, useState } from "react"
import { useRouter, useSearchParams } from "next/navigation"
import { Loader2, MailCheck } from "lucide-react"
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Separator } from "@/components/ui/separator"
import { Button } from "@/components/ui/button"
import {
  InputOTP,
  InputOTPGroup,
  InputOTPSlot,
} from "@/components/ui/input-otp"


const CODE_LENGTH = 6
const RESEND_SECONDS = 60


function VerifyEmailContent() {
  const router = useRouter()
  const searchParams = useSearchParams()

  const email = searchParams.get("email") ?? ""

  const [code, setCode] = useState("")
  const [isLoading, setIsLoading] = useState(false)
  const [isResending, setIsResending] = useState(false)
  const [error, setError] = useState("")
  const [success, setSuccess] = useState("")
  const [seconds, setSeconds] = useState(RESEND_SECONDS)

  useEffect(() => {
    if (seconds <= 0) return

    const timer = setInterval(() => {
      setSeconds((prev) => prev - 1)
    }, 1000)

    return () => clearInterval(timer)
  }, [seconds])

  const handleVerify = async () => {
    if (code.length !== CODE_LENGTH) {
      setError("6 rəqəmli kodu tam daxil edin.")
      return
    }

    try {
      setIsLoading(true)
      setError("")

      // API / Server Action
      //
      // const response = await verifyEmail({
      //   email,
      //   code,
      // })

      console.log({
        email,
        code,
      })

      // Uğurlu verification-dan sonra:
      // router.replace("/login")

    } catch (error) {
      console.error(error)
      setError("Kod düzgün deyil və ya vaxtı bitib.")
    } finally {
      setIsLoading(false)
    }
  }

  const handleResend = async () => {
    if (seconds > 0 || isResending) return

    try {
      setIsResending(true)
      setError("")
      setSuccess("")

      // API / Server Action
      //
      // await resendVerificationCode({ email })

      await new Promise((resolve) => setTimeout(resolve, 800))

      setSeconds(RESEND_SECONDS)
      setSuccess("Yeni təsdiq kodu email ünvanınıza göndərildi.")
    } catch (error) {
      console.error(error)
      setError("Kod yenidən göndərilə bilmədi.")
    } finally {
      setIsResending(false)
    }
  }

  return (
    <main className="flex min-h-screen items-center justify-center bg-muted/40 px-4">
      <Card className="w-full max-w-md shadow-sm">
        <CardHeader className="items-center space-y-3 text-center">
          <div className="flex h-12 w-12 items-center justify-center rounded-full bg-primary/10">
            <MailCheck className="h-6 w-6 text-primary" />
          </div>

          <div className="space-y-1">
            <CardTitle className="text-2xl">
              Verify your email
            </CardTitle>

            <CardDescription>
              Enter the 6-digit code we sent to your email address.
            </CardDescription>
          </div>
        </CardHeader>

        <Separator />

        <CardContent className="pt-6">
          <div className="space-y-6">
            {email && (
              <p className="text-center text-sm text-muted-foreground">
                Kod göndərildi:{" "}
                <span className="font-medium text-foreground">
                  {email}
                </span>
              </p>
            )}

            <div className="flex justify-center">
              <InputOTP
                maxLength={CODE_LENGTH}
                value={code}
                onChange={(value) => {
                  setCode(value)
                  setError("")
                }}
                disabled={isLoading}
                onComplete={handleVerify}
              >
                <InputOTPGroup>
                  <InputOTPSlot index={0} />
                  <InputOTPSlot index={1} />
                  <InputOTPSlot index={2} />
                  <InputOTPSlot index={3} />
                  <InputOTPSlot index={4} />
                  <InputOTPSlot index={5} />
                </InputOTPGroup>
              </InputOTP>
            </div>

            {error && (
              <p
                role="alert"
                className="text-center text-sm text-destructive"
              >
                {error}
              </p>
            )}

            {success && (
              <p
                role="status"
                className="text-center text-sm text-green-600"
              >
                {success}
              </p>
            )}

            <Button
              type="button"
              className="w-full"
              onClick={handleVerify}
              disabled={isLoading || code.length !== CODE_LENGTH}
            >
              {isLoading && (
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
              )}

              {isLoading ? "Verifying..." : "Verify email"}
            </Button>
          </div>
        </CardContent>

        <Separator />

        <CardFooter className="flex flex-col gap-3 pt-6">
          <p className="text-sm text-muted-foreground">
            Kod gəlmədi?
          </p>

          <Button
            type="button"
            variant="ghost"
            onClick={handleResend}
            disabled={seconds > 0 || isResending}
          >
            {isResending ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                Sending...
              </>
            ) : seconds > 0 ? (
              `Resend code in ${seconds}s`
            ) : (
              "Resend code"
            )}
          </Button>

          <Button
            type="button"
            variant="link"
            onClick={() => router.replace("/login")}
            disabled={isLoading || isResending}
          >
            Back to login
          </Button>
        </CardFooter>
      </Card>
    </main>
  )
}
export default function VerifyEmailPage() {
  return (
    <Suspense fallback={null}>
      <VerifyEmailContent />
    </Suspense>
  )
}