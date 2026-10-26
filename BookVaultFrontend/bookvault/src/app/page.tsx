import { redirect } from "next/navigation";
export default function Home() {
  redirect("/login")
  return (
    <div className="flex flex-col flex-1 items-center justify-center bg-zinc-50 font-sans ">
      <section className="max-w-6xl mx-auto px-4 sm:px-8 h-screen -mt-20 grid lg-grid-cols-[1fr, 400px] items-center">
        
      </section>
    </div>
  );
}
