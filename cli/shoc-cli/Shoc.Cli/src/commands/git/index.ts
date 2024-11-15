import { createCommand } from 'commander';
import resolveContext from "@/services/context-resolver";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { getGitDetails } from '@/services/git';

const gitCommand = createCommand('git');

gitCommand.description('Git status')
    .action(asyncHandler(async (options, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));

        const details = await getGitDetails(context.dir)

        console.log("resolved", details)
        
    }));


export default gitCommand;