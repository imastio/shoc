import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import { shocClient } from "@/clients/shoc";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import clientGuard from "@/services/client-guard";
import WorkspaceClustersClient from "@/clients/shoc/cluster/workspace-clusters-client";

const clustersListCommand = createCommand('list')

clustersListCommand
    .description('List my clusters')
    .action(asyncHandler(async (_, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));

        const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, context.workspace));

        const result: any[] = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceClustersClient).getAll(ctx.token, workspace.id));
        
        logger.just("Available clusters:")
        logger.break()
        result.forEach(item => {
            logger.just(`  - Name: ${item.name}`)
            logger.just(`    Description: ${item.description || 'N/A'}`)
            logger.just(`    Status: ${item.status}`)
            logger.break()
        });
    }));


export default clustersListCommand;
