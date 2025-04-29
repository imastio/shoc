"use client"

import { cn } from "@/lib/utils";
import { useIntl } from "react-intl";

export default function NoJobs({ workspaceId, className }: { workspaceId: string, className?: string }){
    const intl = useIntl();

    return <div className={cn("flex flex-1 h-full items-center justify-center rounded-lg border border-dashed shadow-sm", className)}>
    <div className="flex flex-col items-center gap-1 text-center">
        <h3 className="text-2xl font-bold tracking-tight">{intl.formatMessage({id: 'jobs.noJobs'})}</h3>
        <p className="text-sm text-muted-foreground">{intl.formatMessage({id: 'jobs.noJobs.note'})}</p>
    </div>
</div>
}