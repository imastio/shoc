import { rpc } from "@/server-actions/rpc";

export const getWorkspaceByName = (name: string) => {
    console.log("getWorkspaceByName invoked")
    return rpc('workspace/user-workspaces/getByName', { name })
}

export const getWorkspacePermissionsByName = (name: string) => {
    console.log("getWorkspacePermissionsByName invoked")
    return rpc('workspace/user-workspaces/getPermissionsByName', { name })
}