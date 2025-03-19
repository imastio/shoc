import { createCommand } from 'commander';
import { asyncHandler, getRootOptions } from '../common';
import resolveContext from '@/services/context-resolver';
import clientGuard from '@/services/client-guard';
import { shocClient } from '@/clients/shoc';
import WorkspaceJobsClient from '@/clients/shoc/job/workspace-jobs-client';
import UserWorkspacesClient from '@/clients/shoc/workspace/user-workspaces-client';
import WorkspaceJobTasksClient from '@/clients/shoc/job/workspace-job-tasks-client';
import { logger } from '@/services/logger';

const logsCommand = createCommand('logs');

logsCommand.description('Watch logs of the job tasks')
    .argument('<jobKey>', 'The number of job to watch', (value) => {
        const parsedValue = parseInt(value, 10);
        if (isNaN(parsedValue)) {
            throw new Error('Job must be an integer.');
        }
        return parsedValue;
    })
    .option('-t, --task <number>', 'The task number', '0')
    .action(asyncHandler(async (jobKey, options, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));
        const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, context.workspace));

        const task = options.task ?? 0;

        const job = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceJobsClient).getByLocalId(ctx.token, workspace.id, jobKey))

        const { url: logsUrl, token } = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceJobTasksClient).getLogsBySequenceUrl(ctx.token, workspace.id, job.id, task));
        
        await getLogs(logsUrl, token)
    }));

async function getLogs(logsUrl: string, token: string) {

    const response = await fetch(logsUrl, {
        headers: {
            'x-shoc-sse': 'yes',
            Authorization: `Bearer ${token}`
        },
    });
    if (!response.ok) {

        if(response.status === 404){
            throw Error('The object is was not found')
        }

        if(response.status === 403){
            throw Error('No enough permissions for the operation')
        }
        
        const { errors } = (await response.json()) ?? { errors: [] };
        throw Error(errors.length > 0 ? `${errors[0].code} ${errors[0]?.message}` : 'Unknown error');
    }

    if (!response.body) {
        return;
    }

    const reader = response.body.getReader();
    const decoder = new TextDecoder();

    while (true) {
        const { done, value } = await reader.read();

        if (done) {
            break;
        }

        const chunk = decoder.decode(value, { stream: true });

        const events = chunk.split('\n\n');
        for (const event of events) {
            if (event.startsWith('data: ')) {
                logger.just(event.slice(6));
            }
        }
    }
};

export default logsCommand;