"use client"

import useWorkspaceAccess from "@/providers/workspace-access/use-workspace-access";
import WorkspaceClustersCountCard from "./workspace-clusters-count-card";
import { WorkspacePermissions } from "@/well-known/workspace-permissions";

export default function DashboardClientPage({ workspaceId, workspaceName }: { workspaceId: string, workspaceName: string }) {

    const { hasAny } = useWorkspaceAccess();

    return <div className="flex mx-auto w-full flex-col gap-4">
        <div className="flex flex-row">
            { hasAny([WorkspacePermissions.WORKSPACE_LIST_CLUSTERS]) && <WorkspaceClustersCountCard workspaceId={workspaceId} workspaceName={workspaceName} />}
        </div>

    </div>
}