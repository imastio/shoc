import { createCommand } from 'commander';
import resolveContext from "@/services/context-resolver";
import { asyncHandler, getRootOptions } from "@/commands/common";
import build from '@/services/build-service';
import { ResolvedContext } from '@/core/types';
import { BuildContext } from '@/services/build-service/types';

const buildCommand = createCommand('build');

buildCommand.description('Build a package from the selected directory')
    .option('-f, --build-file <buildFile>', 'The name of the build file to use')
    .option('--scope <scope>', 'The scope of the build (user or workspace)', 'workspace')
    .action(asyncHandler(async (options, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));

        const buildContext = getBuildContext(context, options);

        const result = await build(context, buildContext);
    }));

function getBuildContext(context: ResolvedContext, options: any): BuildContext {
    return {
        workspace: context.workspace,
        dir: context.dir,
        buildFile: options.buildFile,
        scope: options.scope
    }
}

export default buildCommand;