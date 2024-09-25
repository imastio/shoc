import { ResolvedContext, RootOptions } from "@/core/types";
import { loadConfig } from "./config";
import axios from "axios";

export default async function resolveContext(options: RootOptions): Promise<ResolvedContext> {

    const { context, workspace, dir } = options;
    const config = await loadConfig();
    
    if(!config){
        throw new Error('The Shoc CLI is not configured. You can run "shoc config init" to configure it.');
    }

    const effectiveContext = context ?? config.defaultContext;
    const ctx = config.contexts.filter(item => item.name === effectiveContext)[0];

    if(!ctx){
        throw new Error(`The context '${effectiveContext}' could not be resolved. Please check the configuration.`);
    }

    const provider = config.providers.filter(item => item.name === ctx.provider)[0];

    if(!provider){
        throw new Error(`The provider '${ctx.provider}' from the context '${ctx.name}' could not be resolved. Please check the configuration.`);
    }

    if(!URL.canParse(provider.url)){
        throw new Error(`The provider ${provider.name} has invalid URL (${provider.url})`);
    }

    return {
        name: ctx.name,
        providerName: provider.name,
        providerUrl: new URL(provider.url),
        workspace: workspace ?? ctx.workspace,
        dir: dir ?? process.cwd()
    };
}

export async function getWellKnownEndpoints(providerUrl: URL): Promise<{ idp: URL, api: URL }> {

    const endpointsUrl = new URL(providerUrl);
    endpointsUrl.pathname = '/well-known/endpoints';
    
    const result = (await axios.get(endpointsUrl.toString())).data;

    return {
        idp: new URL(result.idp),
        api: new URL(result.api)
    }
}