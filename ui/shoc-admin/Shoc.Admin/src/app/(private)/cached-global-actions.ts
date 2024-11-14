import { rpc } from "@/server-actions/rpc";
import { cache } from "react";

export const getEffectiveAccesses = cache(() => {
    return rpc('identity/current-user/getAll')
})
