import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import { shocClient } from "@/clients/shoc";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import clientGuard from "@/services/client-guard";
import WorkspaceJobsClient from "@/clients/shoc/job/workspace-jobs-client";
import { table } from "table";
import { durationBetween } from "@/extended/format";
import WorkspaceJobTasksClient from "@/clients/shoc/job/workspace-job-tasks-client";

const jobTasksCommand = createCommand('tasks')

jobTasksCommand
    .description('Show tasks of the job')
    .requiredOption('-j, --job <number>', 'The job number')
    .option('--json', 'The raw json object')
    .action(asyncHandler(async (options, cmd) => {

        const jobKey = parseInt(options.job, 10);

        if (!Number.isSafeInteger(jobKey)) {
            throw Error(`The ${options.job} is not valid key for job`)
        }

        const context = await resolveContext(getRootOptions(cmd));

        const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, context.workspace));

        const job: any = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceJobsClient).getByLocalId(ctx.token, workspace.id, jobKey));
        const tasks: any[] = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceJobTasksClient).getAll(ctx.token, workspace.id, job.id));

        if (options.json) {
            logger.just(JSON.stringify(tasks, null, 4))
            return;
        }

        const header = ['Sequence', 'Status', 'Cluster', 'User', 'Type', 'Waiting', 'Running'];
        const rows = [header];

        tasks.forEach((item: any) => rows.push([
            item.sequence ?? '',
            item.status ?? '',
            item.clusterName ?? '',
            item.userFullName ?? '',
            item.type ?? '',
            durationBetween(item.created, item.runningAt),
            item.runningAt ? durationBetween(item.runningAt, item.completedAt) : "N/A"
        ]))

        logger.just(table(rows, {
            drawHorizontalLine: () => false,
            drawVerticalLine: () => false
        }));
    }));


export default jobTasksCommand;
