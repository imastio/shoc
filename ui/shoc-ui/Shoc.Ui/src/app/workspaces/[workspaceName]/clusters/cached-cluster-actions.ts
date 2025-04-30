import { rpc } from "@/server-actions/rpc";
import { cache } from "react";

export const getClusterByName = cache((workspaceId: string, name: string) => {
    return rpc('cluster/workspace-clusters/getByName', { workspaceId, name })
})

export const getClusterPermissionsByName = cache((workspaceId: string, name: string) => {
    return rpc('cluster/workspace-clusters/getPermissionsByName', { workspaceId, name })
})
