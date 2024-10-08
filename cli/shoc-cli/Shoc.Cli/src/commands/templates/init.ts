import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, DEFAULT_BUILD_FILE, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import { anonymousClientGuard } from "@/services/client-guard";
import { shocClient } from "@/clients/shoc";
import TemplatesClient from "@/clients/shoc/package/templates-client";
import fs from "fs";
import { stringify as yamlStringify } from 'yaml';
import { BuildManifest } from "../jobs/_functions/types";

const templatesInitCommand = createCommand('init')

templatesInitCommand
    .description('Initialize the target folder with the selected template')
    .argument('<template>', 'The template name and variant to initialize. Example: alpine:default.')
    .action(asyncHandler(async (template, _, cmd) => {
        
        const context = await resolveContext(getRootOptions(cmd));

        const templateSplit = template.split(':', 2);
        const name = templateSplit[0].trim();
        const variant = (templateSplit[1] || 'default').trim();

        const spec = await anonymousClientGuard(context, (ctx) => shocClient(ctx.apiRoot, TemplatesClient).getBuildSpecInstanceByName(name, variant));
    
        logger.info(`Initializing the template in the current directory (${context.dir})...`)

        const buildObject: BuildManifest = {
            template: `${name}:${variant}`,
            spec: spec,
            ignore: []
        }

        const targetBuildFile = `${context.dir}/${DEFAULT_BUILD_FILE}`

        if(fs.existsSync(targetBuildFile)){
            throw Error(`The build file ${DEFAULT_BUILD_FILE} already exists!`)
        }

        const buildYaml = yamlStringify(buildObject);
        fs.writeFileSync(targetBuildFile, buildYaml)
        logger.success(`Initialized the build file (${DEFAULT_BUILD_FILE}) with the template ${name}:${variant}`)        
    }));


export default templatesInitCommand;
