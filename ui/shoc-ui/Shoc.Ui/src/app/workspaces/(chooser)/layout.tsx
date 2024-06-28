import AppHeader from "@/components/layout/app-header";

export default async function WorkspacesLayout({ children }: any) {
  return <>
    <AppHeader mobileSidebar={false} />
    <main className="flex h-full">
      {children}
    </main>
  </>
}
