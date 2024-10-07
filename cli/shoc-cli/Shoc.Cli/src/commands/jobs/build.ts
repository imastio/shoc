import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import { computeListingHash, createZip, getBuildListing, getBuildObject } from "./_functions";

const jobsBuildCommand = createCommand('build')

jobsBuildCommand
    .description('Build a job package from the selected directory')
    .option('-f, --build-file <buildFile>', 'The name of the build file to use')
    .action(asyncHandler(async (options, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));
        const { buildFile, buildObject } = await getBuildObject(context, options);
        
        const { files } = await getBuildListing(context, buildObject);

        const hash = computeListingHash(buildFile, files);

        const zip = await createZip(files);

    }));


export default jobsBuildCommand;
