"use client"

import useWorkspaceAccess from "@/providers/workspace-access/use-workspace-access";
import WorkspaceClustersCountCard from "./workspace-clusters-count-card";
import { WorkspacePermissions } from "@/well-known/workspace-permissions";
import DashboardJobsCard from "./dashboard-jobs-card";

export default function DashboardClientPage({ workspaceId, workspaceName }: { workspaceId: string, workspaceName: string }) {

    const { hasAny } = useWorkspaceAccess();

    return <div className="flex flex-1 flex-col gap-4 p-4 pt-0">
    <div className="grid auto-rows-min gap-4 md:grid-cols-2">
      <div className="rounded-xl bg-muted/50">
        <DashboardJobsCard />
      </div>
      <div className="rounded-xl bg-muted/50" />
    </div>
    <div className="min-h-[100vh] flex-1 rounded-xl bg-muted/50 md:min-h-min" />
  </div>
}