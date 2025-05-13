"use client"

import { useMemo } from "react";
import ClusterContext from "./cluster-context";

export default function ClusterProvider({ children, cluster } : { children: React.ReactNode, cluster: any }){
    
    const value = useMemo(() => ({
        id: cluster.id,
        workspaceId: cluster.workspaceId,
        workspaceName: cluster.workspaceName,
        name: cluster.name,
        description: cluster.description,
        type: cluster.type,
        status: cluster.status,
        created: cluster.created,
        updated: cluster.updated
    }), [cluster])

    return <ClusterContext.Provider value={value}>
        {children}
    </ClusterContext.Provider>
}