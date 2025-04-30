import AppHeader from "@/components/layout/app-header";
import WorkspaceMobileSidebar from "@/components/workspace/workspace-mobile-sidebar";
import WorkspaceSidebar from "@/components/workspace/workspace-sidebar";
import { ReactNode } from "react";
import { getByName, getPermissionsByName } from "./cached-workspace-actions";
import ErrorScreen from "@/components/error/error-screen";
import WorkspaceAccessProvider from "@/providers/workspace-access/workspace-access-provider";
import { Metadata } from "next";

export async function generateMetadata({ params: { workspaceName } }: { params: any }): Promise<Metadata> {

    const { data } = await getByName(workspaceName);

    return {
        description: data?.description || ''
    }
}

export default async function SingleWorkspaceLayout({ params: { workspaceName }, children }: { children: ReactNode, params: any }) {

    const [workspace, permissions] = await Promise.all([getByName(workspaceName), getPermissionsByName(workspaceName)])

    if (workspace.errors || permissions.errors) {
        return <ErrorScreen errors={workspace.errors || permissions.errors} />
    }

    return <WorkspaceAccessProvider permissions={permissions.data || []}>
        <AppHeader mobileSidebar={<WorkspaceMobileSidebar name={workspaceName} />} />
        <main className="flex h-full">
            <WorkspaceSidebar name={workspaceName} />
            {children}
        </main>
    </WorkspaceAccessProvider>
}