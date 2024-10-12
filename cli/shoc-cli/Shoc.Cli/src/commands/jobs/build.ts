import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { computeListingHash, createZip, getBuildListing, getBuildManifest } from "./_functions";
import clientGuard from "@/services/client-guard";
import { shocClient } from "@/clients/shoc";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import WorkspaceBuildTasksClient from "@/clients/shoc/package/build-tasks-client";
import fs from "fs";
import { FormData } from 'formdata-node';
import { fileFromPath } from 'formdata-node/file-from-path';


const jobsBuildCommand = createCommand('build')

jobsBuildCommand
    .description('Build a job package from the selected directory')
    .option('-f, --build-file <buildFile>', 'The name of the build file to use')
    .action(asyncHandler(async (options, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));
        const { buildFile, manifest } = await getBuildManifest(context, options);
        
        const { files } = await getBuildListing(context, manifest);

        const hash = computeListingHash(buildFile, files);
        
        const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, context.workspace));

        const input = {
            workspaceId: workspace.id,
            provider: 'remote',
            scope: 'workspace',
            listingChecksum: hash,
            manifest: JSON.stringify(manifest)
        }

        const task = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceBuildTasksClient).create(ctx.token, workspace.id, input));
        
        const zip = await createZip(files);

        const uploadData = new FormData()
        uploadData.append('file', await fileFromPath(zip))
        
        const uploaded = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceBuildTasksClient).uploadBundleById(ctx.token, workspace.id, task.id, uploadData));

    }));


export default jobsBuildCommand;
