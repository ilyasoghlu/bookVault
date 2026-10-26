import { Navbar, Sidebar, Footer } from "@/components/layout";
import Container from "@/components/global/Container";
import AuthGuard from "@/components/auth/AuthGuard";

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <AuthGuard>
      <div className="w-full">
        <div className="flex sm:flex-row flex-col">
            {/* Sidebar */}
          <div
            className="
              hidden
              sm:block sm:max-w-68
              lg:block xl:block
              w-full rounded-2xl
            "
          >
            <Sidebar />
          </div>
            {/* Main content */}
          <div className="flex flex-col w-full min-h-screen">
            <Navbar />
            <main className="flex-1">
              <Container className="w-full m-2">
                {children}
              </Container>
            </main>
            <Footer />
          </div>
        </div>
      </div>
    </AuthGuard>
  );
}
