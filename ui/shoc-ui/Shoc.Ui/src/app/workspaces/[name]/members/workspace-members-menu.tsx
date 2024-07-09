"use client"

import { cn } from "@/lib/utils";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { useMemo } from "react";
import { useIntl } from "react-intl";

export default function WorkspaceMembersMenu({ name }: { name: string }) {

    const intl = useIntl();
    const pathname = usePathname();

    const definitions = useMemo(() => [
        {
            path: `/workspaces/${name}/members`,
            title: intl.formatMessage({ id: 'workspaces.members.menu.members' })
        },
        {
            path: `/workspaces/${name}/members/invitations`,
            title: intl.formatMessage({ id: 'workspaces.members.menu.invitations' })
        }
    ], [intl, name])

    const menu = useMemo(() => definitions.map(item => ({
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