import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import { shocClient } from "@/clients/shoc";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import clientGuard from "@/services/client-guard";
import WorkspaceJobsClient from "@/clients/shoc/job/workspace-jobs-client";
import { table } from "table";
import chalk from "chalk";
import { localDateTimeWithSec } from "@/extended/format";

const jobDetailsCommand = createCommand('details')

jobDetailsCommand
    .description('Show details of the job')
    .requiredOption('-j, --job <number>', 'The job number')
    .option('--json', 'The raw json object')
    .action(asyncHandler(async (options, cmd) => {

        const jobKey = parseInt(options.job, 10);

        if (!Number.isSafeInteger(jobKey)) {
            throw Error(`The ${options.job} is not valid key for job`)
        }

        const context = await resolveContext(getRootOptions(cmd));

        const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, context.workspace));

        const result: any = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceJobsClient).getByLocalId(ctx.token, workspace.id, jobKey));

        if (options.json) {
            logger.just(JSON.stringify(result, null, 4))
            return;
        }

        const rows = [
            [chalk.bold('Name'), chalk.bold('Value')],
            ['Id', result.id],
            ['Job Key', result.localId],
            ['Workspace', result.workspaceName],
            ['Cluster', result.clusterName],
            ['User', result.userFullName],
            ['Scope', result.scope],
            ['Namespace', result.namespace],
            ['Status', result.status],
            ['Tasks', result.totalTasks],
            ['Completed Tasks', result.completedTasks],
            ['Succeeded Tasks', result.succeededTasks],
            ['Failed Tasks', result.failedTasks],
            ['Cancelled Tasks', result.cancelledTasks],
            ['Pending Since', localDateTimeWithSec(result.pendingAt)],
            ['Running Since', localDateTimeWithSec(result.runningAt)],
            ['Completed At', localDateTimeWithSec(result.completedAt)],
        ];

        logger.just(table(rows, {
            drawHorizontalLine: () => false,
            drawVerticalLine: () => false
        }));
    }));


export default jobDetailsCommand;
