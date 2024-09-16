"use client"

import BasicHeader from "@/components/general/basic-header";
import { useIntl } from "react-intl"
import WorkspaceMembersCountCard from "./workspace-members-count-card";
import useWorkspaceAccess from "@/providers/workspace-access/use-workspace-access";

export default function DashboardClientPage({ workspaceId, workspaceName }: { workspaceId: string, workspaceName: string }) {

    const intl = useIntl();
    const { hasAny } = useWorkspaceAccess();

    return <div className="flex mx-auto w-full flex-col gap-4 p-4 lg:gap-6 lg:p-6">
        <BasicHeader
            title={intl.formatMessage({ id: 'workspaces.sidebar.dashboard' })}
        />

        <div className="flex flex-row">
            { hasAny(['workspace_list_members']) && <WorkspaceMembersCountCard workspaceId={workspaceId} workspaceName={workspaceName} />}
        </div>

    </div>
}