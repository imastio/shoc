import { createCommand } from 'commander';
import { asyncHandler, getRootOptions } from '../common';
import resolveContext from '@/services/context-resolver';
import clientGuard from '@/services/client-guard';
import { shocClient } from '@/clients/shoc';
import WorkspaceJobsClient from '@/clients/shoc/job/workspace-jobs-client';
import UserWorkspacesClient from '@/clients/shoc/workspace/user-workspaces-client';
import { logger } from '@/services/logger';
import { table } from 'table';
import { durationBetween } from '@/extended/format';
import jobDetailsCommand from './details';
import jobTasksCommand from './tasks';

const jobsCommand = createCommand('jobs').aliases(['job']);

jobsCommand.addCommand(jobDetailsCommand);
jobsCommand.addCommand(jobTasksCommand);

jobsCommand.description('List available jobs')
    .option('--page <number>', 'The page number', '0')
    .option('--size <number>', 'The size of the result', '10')
    .option('--status <string>', 'The status to filter', '')
    .option('--scope <string>', 'The scope to filter', '')
    .option('--all', 'Consider all jobs in the workspace', 'true')
    .action(asyncHandler(async (options, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));
        const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, context.workspace));
        
        const page = options.page ?? 0;
        const size = options.size ?? 20;
        const all = options.all;
        const status = options.status;
        const scope = options.scope;

        const filter = {
            status, 
            scope,
            all,
            page,
            size
        }

        const { items, totalCount } = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceJobsClient).getBy(ctx.token, workspace.id, filter))

        const header = ['Job', 'Status', 'Cluster', 'User', 'Completed', 'Scope', 'Waiting', 'Running'];
        const rows = [header];

        items.forEach((item: any) => rows.push([
            item.localId ?? '',
            item.status ?? '',
            item.clusterName ?? '',
            item.userFullName ?? '',
            `${item.completedTasks} / ${item.totalTasks}`,
            item.scope,
            durationBetween(item.created, item.runningAt),
            item.runningAt ? durationBetween(item.runningAt, item.completedAt) : "N/A"
        ]))
        

        logger.just(table(rows, { 
            drawHorizontalLine: () => false, 
            drawVerticalLine: () => false 
        }));

        logger.just(`Current page ${page}: ${items.length} jobs out of ${totalCount}`)
    }));

export default jobsCommand;