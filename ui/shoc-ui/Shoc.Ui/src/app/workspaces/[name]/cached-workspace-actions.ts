import { rpc } from "@/server-actions/rpc";
import { cache } from "react";

export const getByName = cache((name: string) => {
    console.log("getByName invoked")
    return rpc('workspace/user-workspaces/getByName', { name })
})

export const getPermissionsByName = cache((name: string) => {
    console.log("getPermissionsByName invoked")
    return rpc('workspace/user-workspaces/getPermissionsByName', { name })
})

export const getMembersById = cache((id: string) => {
    console.log("getMembersById invoked")
    return rpc('workspace/user-workspace-members/getAll', { workspaceId: id })
})