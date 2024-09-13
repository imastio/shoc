'use server'

import { ServerActionContext, ServerActionInput, ServerActionResult } from "./types";
import allRpc from "./registry";
import toServerActionErrors from "@/addons/error-handling/error-utility";

type ServerActionName = keyof typeof allRpc

function rpcImpl(name: ServerActionName, input?: ServerActionInput, context?: ServerActionContext): Promise<any> {
    const resolvedRpc = allRpc[name];

    if(!resolvedRpc){
        return allRpc['index/noServerAction'](input || {}, context || {});
    }

    return resolvedRpc(input || {}, context || {});
}

export async function rpc(name: ServerActionName, input?: ServerActionInput, context?: ServerActionContext): Promise<ServerActionResult> {

    try {
        return {
            data: await rpcImpl(name, input, context),
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
    return await rpcImpl(name, input, context);
}
