import { ResolvedContext } from "@/core/types";
import { loadConfig } from "./config";
import ErrorDefinitions from "@/error-handling/error-definitions";

export default async function resolveContext(context?: string, workspace?: string): Promise<ResolvedContext> {

    var config = await loadConfig();
    
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
        workspace: workspace ?? ctx.workspace
    };
}