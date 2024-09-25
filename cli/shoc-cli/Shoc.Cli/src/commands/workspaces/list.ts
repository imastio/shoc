import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import { shocClient } from "@/clients/shoc";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import clientGuard from "@/services/client-guard";

const workspacesListCommand = createCommand('list')

workspacesListCommand
    .description('List my workspaces')
    .action(asyncHandler(async (_, cmd) => {

        const rootOptions = getRootOptions(cmd);
        const context = await resolveContext(rootOptions.context, rootOptions.workspace);

        const result: any[] = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getAll(ctx.token));
        
        logger.just("Available workspaces:")
        logger.break()
        result.forEach(item => {
            logger.just(`  - Name: ${item.name}`)
            logger.just(`    Description: ${item.description}`)
            logger.just(`    Role: ${item.role}`)
            logger.break()
        });
    }));


export default workspacesListCommand;
