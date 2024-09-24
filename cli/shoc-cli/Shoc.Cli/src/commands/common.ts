import { ResolvedContext } from "@/core/types";
import { logger } from "@/services/logger";
import axios from "axios";
import https from 'https';
import { Command } from "commander";

export function getRootCommand(command: Command) {

    let current = command;

    while (current.parent) {
        current = current.parent;
    }

    return current;
}

export function getRootOptions(command: Command): { context?: string, workspace?: string } {
    const root = getRootCommand(command);
    const opts = root.opts();

    return {
        context: opts.context,
        workspace: opts.workspace
    }
}

export async function getWellKnownEndpoints(context: ResolvedContext): Promise<{ idp: URL, api: URL }> {

    const url = context.providerUrl;

    const endpointsUrl = new URL(url);
    endpointsUrl.pathname = '/well-known/endpoints';
    
    const result = await (await fetch(endpointsUrl)).json();

    return {
        idp: new URL(result.idp),
        api: new URL(result.api)
    }
}

export const asyncHandler = (fn: (...args: any[]) => Promise<void>) => (...args: any[]) => {
    fn(...args).catch((err) => {
        logger.error(`${err.message}`);
        process.exit(1);
    });
};