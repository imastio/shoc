"use client"

import { Button } from "@/components/ui/button";
import { useIntl } from "react-intl";

export default function NoClusters(){
    const intl = useIntl();

    return <div className="flex flex-1 h-full items-center justify-center rounded-lg border border-dashed shadow-sm">
    <div className="flex flex-col items-center gap-1 text-center">
        <h3 className="text-2xl font-bold tracking-tight">{intl.formatMessage({id: 'workspaces.clusters.noWorkspaces'})}</h3>
        <p className="text-sm text-muted-foreground">{intl.formatMessage({id: 'workspaces.clusters.noWorkspaces.note'})}</p>
        <Button className="mt-4" />
    </div>
</div>
}