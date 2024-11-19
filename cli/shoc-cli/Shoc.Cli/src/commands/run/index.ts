import { createCommand } from 'commander';
import resolveContext from "@/services/context-resolver";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { ResolvedContext } from '@/core/types';
import { RunContext } from '@/services/run-service/types';
import run from '@/services/run-service';

const runCommand = createCommand('run');

runCommand.description('Submit a new job according to run manifest')
    .option('--build-file <buildFile>', 'The name of the build manifest file to use')
    .option('--run-file <runFile>', 'The name of the run manifest file to use')
    .option('--scope <scope>', 'The scope of the build (user or workspace)', 'workspace')
    .action(asyncHandler(async (options, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));

        const runContext = getRunContext(context, options);

        const result = await run(context, runContext);
    }));

function getRunContext(context: ResolvedContext, options: any): RunContext {
    return {
        workspace: context.workspace,
        dir: context.dir,
        buildFile: options.buildFile,
        scope: options.scope,
        runFile: options.runFile
    }
}

export default runCommand;