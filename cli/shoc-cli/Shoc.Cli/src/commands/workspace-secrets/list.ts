import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import { shocClient } from "@/clients/shoc";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import clientGuard from "@/services/client-guard";
import WorkspaceSecretsClient from "@/clients/shoc/secret/workspace-secrets-client";

const workspaceSecretsListCommand = createCommand('list')

workspaceSecretsListCommand
    .description('List workspace secrets')
    .action(asyncHandler(async (_, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));

        const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, context.workspace));

        const result: any[] = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceSecretsClient).getAll(ctx.token, workspace.id));
        
        logger.just("List of defined workspace secrets:")
        logger.break()
        result.forEach(item => {
            logger.just(`  - Name: ${item.name}, Encrypted: ${item.encrypted ? '✓' : '✗'}, Value: ${item.value}`)
        });
    }));


export default workspaceSecretsListCommand;
