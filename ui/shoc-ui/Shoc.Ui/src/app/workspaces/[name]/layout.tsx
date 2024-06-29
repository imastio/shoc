import AppHeader from "@/components/layout/app-header";
import WorkspaceMobileSidebar from "@/components/workspace/workspace-mobile-sidebar";
import WorkspaceSidebar from "@/components/workspace/workspace-sidebar";
import { ReactNode } from "react";
import { getByName, getPermissionsByName } from "./cached-workspace-actions";
import ErrorScreen from "@/components/error/error-screen";
import WorkspaceAccessProvider from "@/providers/workspace-access/workspace-access-provider";
import { Metadata } from "next";

export async function generateMetadata({ params: { name } }: { params: any }): Promise<Metadata> {

    const { data } = await getByName(name);
  
    return {
      description: data?.description || ''
    }
  }

export default async function SingleWorkspaceLayout({ params: { name }, children }: { children: ReactNode, params: any }) {

    const [workspace, permissions] = await Promise.all([getByName(name), getPermissionsByName(name)])

    if (workspace.errors || permissions.errors) {
        return <ErrorScreen errors={workspace.errors || permissions.errors} />
    }

    return <WorkspaceAccessProvider permissions={permissions.data || []}>
        <AppHeader mobileSidebar={<WorkspaceMobileSidebar name={name} />} />
        <main className="flex h-full">
            <WorkspaceSidebar name={name} />
            {children}
        </main>
    </WorkspaceAccessProvider>
}