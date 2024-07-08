import { rpc } from "@/server-actions/rpc";
import { cache } from "react";

export const getByName = cache((name: string) => {
    return rpc('workspace/user-workspaces/getByName', { name })
})

export const getPermissionsByName = cache((name: string) => {
    return rpc('workspace/user-workspaces/getPermissionsByName', { name })
})
