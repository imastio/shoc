import { RootOptions } from "@/core/types";
import { logger } from "@/services/logger";
import { Command } from "commander";

export function getRootCommand(command: Command) {

    let current = command;

    while (current.parent) {
        current = current.parent;
    }

    return current;
}

export function getRootOptions(command: Command): RootOptions {
    const root = getRootCommand(command);
    const opts = root.opts();

    return {
        context: opts.context,
        workspace: opts.workspace,
        dir: opts.dir
    }
}

export const asyncHandler = (fn: (...args: any[]) => Promise<void>) => (...args: any[]) => {
    fn(...args).catch((err) => {
        logger.error(`${err.message}`);
        process.exit(1);
    });
};