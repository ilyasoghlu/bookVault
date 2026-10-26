"use client";

import { useEffect } from "react";
import { usePathname, useRouter } from "next/navigation";

import { useAuth } from "@/hooks/useAuth";

const publicRoutes = [
  "/login",
  "/register",
  "/verify-email",
  "/forgot-password",
];

interface AuthGuardProps {
  children: React.ReactNode;
}

export default function AuthGuard({
  children,
}: AuthGuardProps) {
  const pathname = usePathname();
  const router = useRouter();

  const { user, loading } = useAuth();

  const isPublicRoute = publicRoutes.some(
    (route) =>
      pathname === route ||
      pathname.startsWith(`${route}/`)
  );

  useEffect(() => {
    if (loading) {
      return;
    }

    if (!isPublicRoute && !user) {
      router.replace("/login");
    }
  }, [loading, user, isPublicRoute, router]);

  if (loading && !isPublicRoute) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        Loading...
      </div>
    );
  }

  if (!isPublicRoute && !user) {
    return null;
  }

  return <>{children}</>;
}