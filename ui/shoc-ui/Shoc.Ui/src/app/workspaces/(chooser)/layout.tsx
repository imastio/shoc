import AppHeader from "@/components/layout/app-header";

export default async function WorkspacesLayout({ children }: any) {
  return <div className="grid min-h-screen w-full">

    <div className="flex flex-col w-full">
      <AppHeader mobileSidebar={false} />
      <main className="flex h-full">
        {children}
      </main>
    </div>
  </div>
}
