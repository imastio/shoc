"use client"

import { useIntl } from "react-intl";
import ClusterAddDialogButton from "./cluster-add-dialog-button";

export default function NoClusters({workspaceId, onCreated = () => {}}: {workspaceId: string, onCreated: (result: any) => any}){
    const intl = useIntl();

    return <div className="flex flex-1 h-full items-center justify-center rounded-lg border border-dashed shadow-xs">
    <div className="flex flex-col items-center gap-1 text-center">
        <h3 className="text-2xl font-bold tracking-tight">{intl.formatMessage({id: 'clusters.noClusters'})}</h3>
        <p className="text-sm text-muted-foreground">{intl.formatMessage({id: 'clusters.noClusters.note'})}</p>
        <ClusterAddDialogButton workspaceId={workspaceId} className="mt-4" onSuccess={(result) => onCreated(result)} />
    </div>
</div>
}