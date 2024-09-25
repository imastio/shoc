import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import { shocClient } from "@/clients/shoc";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import clientGuard from "@/services/client-guard";
import WorkspaceUserSecretsClient from "@/clients/shoc/secret/workspace-user-secrets-client";

const userSecretsListCommand = createCommand('list')

userSecretsListCommand
    .description('List my secrets')
    .action(asyncHandler(async (_, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));

        const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, context.workspace));

        const result: any[] = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceUserSecretsClient).getAll(ctx.token, workspace.id));
        
        logger.just("List of defined user secrets:")
        result.forEach(item => {
            logger.just(`  - Name: ${item.name}, Encrypted: ${item.encrypted ? '✓' : '✗'}, Value: ${item.value}`)
        });
    }));


export default userSecretsListCommand;
