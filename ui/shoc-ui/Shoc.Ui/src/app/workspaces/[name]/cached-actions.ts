import { rpc } from "@/server-actions/rpc";
import { cache } from "react";

export const getByName = cache((name: string) => {
    console.log("Calling get by name")
    return rpc('workspace/user-workspaces/getByName', { name })
})