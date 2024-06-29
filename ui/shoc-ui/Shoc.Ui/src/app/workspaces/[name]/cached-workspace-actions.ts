import { rpc } from "@/server-actions/rpc";
import { cache } from "react";

export const getByName = cache((name: string) => {
    console.log("getWorkspaceByName invoked")
    return rpc('workspace/user-workspaces/getByName', { name })
})

export const getPermissionsByName = cache((name: string) => {
    console.log("getWorkspacePermissionsByName invoked")
    return rpc('workspace/user-workspaces/getPermissionsByName', { name })
})