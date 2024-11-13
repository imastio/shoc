import { createCommand } from 'commander';
import resolveContext from "@/services/context-resolver";
import { asyncHandler, getRootOptions } from "@/commands/common";
import git from 'isomorphic-git'
import fs from 'fs'

const gitCommand = createCommand('git');

gitCommand.description('Git status')
    .action(asyncHandler(async (options, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));

        const gitFolder = await git.findRoot({ fs, filepath: context.dir })

        let remotes = await git.listRemotes({ fs, dir: gitFolder })

        console.log("resolved", remotes)
        
    }));


export default gitCommand;