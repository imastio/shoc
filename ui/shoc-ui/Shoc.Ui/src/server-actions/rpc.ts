'use server'

import { ServerActionContext, ServerActionFunction, ServerActionInput, ServerActionResult } from "./types";
import allRpc from "./registry";
import toServerActionErrors from "@/addons/error-handling/error-utility";
import { cache } from "react";

type ServerActionName = keyof typeof allRpc

function rpcImpl(registry: any,name: ServerActionName, input?: ServerActionInput, context?: ServerActionContext): Promise<any> {
    const resolvedRpc: ServerActionFunction = registry[name];

    if(!resolvedRpc){
        return registry['index/noServerAction'](input || {}, context || {});
    }

    return resolvedRpc(input || {}, context || {});
}

export async function rpc(name: ServerActionName, input?: ServerActionInput, context?: ServerActionContext): Promise<ServerActionResult> {

    try {
        return {
            data: await rpcImpl(allRpc, name, input, context),
            errors: null
        }
    }
    catch(error){
        return {
            data: null,
            errors: toServerActionErrors(error)
        }
    }
}

export async function rpcDirect(name: ServerActionName, input?: ServerActionInput, context?: ServerActionContext): Promise<any> {
    return await rpcImpl(allRpc, name, input, context);
}
