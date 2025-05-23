"use client"

import { cn } from "@/lib/utils";
import useWorkspaceAccess from "@/providers/workspace-access/use-workspace-access";
import { WorkspacePermissions } from "@/well-known/workspace-permissions";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { useMemo } from "react";
import { useIntl } from "react-intl";

export default function WorkspaceJobsMenu({ name }: { name: string }) {

    const intl = useIntl();
    const pathname = usePathname();
    const { hasAny } = useWorkspaceAccess();

    const definitions = useMemo(() => [
        {
            path: `/workspaces/${name}/jobs`,
            title: intl.formatMessage({ id: 'jobs.menu.jobs' }),
            visible: hasAny([WorkspacePermissions.WORKSPACE_LIST_ALL_JOBS, WorkspacePermissions.WORKSPACE_LIST_JOBS])
        }
    ], [intl, name, hasAny])

    const menu = useMemo(() => definitions.filter(item => item.visible).map(item => ({
        path: item.path,
        title: item.title,
        active: item.path === pathname
    })), [definitions, pathname])

    return <div className="flex h-12 items-center gap-4 border-b bg-background px-4 mb-4">
        <nav className="gap-3 md:gap-6 flex flex-row md:items-center text-sm">

            {menu.map((item, index) => <Link
                key={index}
                href={item.path}
                prefetch={false}
                className={cn("transition-colors hover:text-foreground", item.active ? "text-foreground" : "text-muted-foreground")}
            >
                {item.title}
            </Link>)}
        </nav>
    </div>
}