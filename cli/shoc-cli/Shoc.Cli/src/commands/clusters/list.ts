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

        const rootOptions = getRootOptions(cmd);
        const context = await resolveContext(rootOptions.context, rootOptions.workspace);

        const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, context.workspace));

        const result: any[] = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceClustersClient).getAll(ctx.token, workspace.id));
        
        logger.just("Available clusters:")
        logger.break()
        logger.table(result)
    }));


export default clustersListCommand;
