import AppHeader from "@/components/layout/app-header";
import WorkspaceMobileSidebar from "@/components/workspace/workspace-mobile-sidebar";
import WorkspaceSidebar from "@/components/workspace/workspace-sidebar";
import { ReactNode } from "react";
import { getByName } from "./cached-actions";

export default async function SingleWorkspaceLayout({ params: { name }, children }: { children: ReactNode, params: any }) {
    const { data } = await getByName(name)
    return <div className="grid min-h-screen w-full">
        <div className="flex flex-col w-full">
            <AppHeader mobileSidebar={<WorkspaceMobileSidebar />} />
            <main className="flex h-full">
                <WorkspaceSidebar />
                {children}
            </main>
        </div>
    </div>
}