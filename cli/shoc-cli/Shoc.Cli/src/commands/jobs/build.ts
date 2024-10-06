import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, DEFAULT_BUILD_FILE, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import fsDirect from "fs";
import { promises as fs } from 'fs';
import * as YAML from 'yaml';
import { glob } from "glob";

const DEFAULT_IGNORE_PATTERNS = ['.git/**', 'node_modules/**']

const jobsBuildCommand = createCommand('build')

jobsBuildCommand
    .description('Build a job package from the selected directory')
    .action(asyncHandler(async (_, cmd) => {
        
        const context = await resolveContext(getRootOptions(cmd));
        const targetBuildFile = `${context.dir}/${DEFAULT_BUILD_FILE}`

        if(!fsDirect.existsSync(targetBuildFile)){
            throw Error(`The build file ${DEFAULT_BUILD_FILE} could not be found!`)
        }

        const file = await fs.readFile(targetBuildFile, 'utf8');
        const buildObject = YAML.parse(file);

        const files = await glob(
            '**/*', 
            { 
                ignore: [...DEFAULT_IGNORE_PATTERNS, ...(buildObject.ignore || [])], 
                cwd: context.dir, nodir: true 
            }
        );

        files.forEach(file => logger.just(file));
                
    }));


export default jobsBuildCommand;
