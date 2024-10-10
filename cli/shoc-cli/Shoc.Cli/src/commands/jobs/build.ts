import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { computeListingHash, createZip, getBuildListing, getBuildManifest } from "./_functions";

const jobsBuildCommand = createCommand('build')

jobsBuildCommand
    .description('Build a job package from the selected directory')
    .option('-f, --build-file <buildFile>', 'The name of the build file to use')
    .action(asyncHandler(async (options, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));
        const { buildFile, manifest } = await getBuildManifest(context, options);
        
        const { files } = await getBuildListing(context, manifest);

        const hash = computeListingHash(buildFile, files);
        
        console.log("Hash computed", { hash, length: hash.length })

        const zip = await createZip(files);

    }));


export default jobsBuildCommand;
