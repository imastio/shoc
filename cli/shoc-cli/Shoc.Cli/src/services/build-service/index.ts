import { ResolvedContext } from "@/core/types";
import { BuildContext } from "./types";
import { computeListingHash, createZip, getBuildListing, getBuildManifest } from "./implementation";
import { shocClient } from "@/clients/shoc";
import clientGuard from "@/services/client-guard";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import WorkspaceBuildTasksClient from "@/clients/shoc/package/workspace-build-tasks-client";
import { fileFromPath } from "formdata-node/file-from-path";
import chalk from "chalk";
import { requireSession } from "@/services/session-service";
import WorkspacePackagesClient from "@/clients/shoc/package/workspace-packages-client";
import { ApiError } from "@/error-handling/error-types";
import ora, { oraPromise } from "ora";

export default async function build(context: ResolvedContext, buildContext: BuildContext) : Promise<{ packageId: string }> {
    
    const session = await oraPromise(requireSession(context.providerUrl.toString()), {
        successText: res => `Authenticated by ${chalk.bold(res.name)} at ${chalk.bold(buildContext.workspace)}`,
        failText: err => `Could not authenticate: ${chalk.red(err.message)}` 
    }).catch(() => process.exit(1));

    ora().succeed(`Using ${chalk.bold(buildContext.dir)} as a package directory`);
    ora().succeed(`Packaing with scope ${chalk.bold(buildContext.scope)}`);
 
    const { buildFile, manifest } = await oraPromise(getBuildManifest(buildContext), {
        successText: res => `Detected build manifest at ${chalk.bold(res.buildFile.fullPath)}`,
        failText: err => `Build manifest could not be found: ${chalk.red(err.message)}` 
    }).catch(() => process.exit(1));

    const { files } = await oraPromise(getBuildListing(buildContext, manifest), {
        successText: res => `Detected ${chalk.bold(res.files.length)} files to package`,
        failText: err => `Could not fetch the list of files to package: ${chalk.red(err.message)}`
    }).catch(() => process.exit(1));

    const hash = computeListingHash(buildFile, files);
    ora().succeed(`Computed checksum of the package ${chalk.bold(hash)}`);

    const workspace = await oraPromise(clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, buildContext.workspace)), {
        text: `Validating workspace ${chalk.bold(buildContext.workspace)}`,
        successText: res => `Workspace ${chalk.bold(res.name)} is valid`,
        failText: err => `The workspace ${chalk.bold(buildContext.workspace)} could not be found: ${chalk.red(err.message)}`
    }).catch(() => process.exit(1));

    const duplicate = await oraPromise(tryCreateFromCache(context, {
        workspaceId: workspace.id,
        scope: buildContext.scope,
        listingChecksum: hash
    }), {
        text: 'Checking if the package could be restored from cache',
        successText: res => res ? 'The package has been already built' : 'The package was not cached',
        failText: err => `Could not perform the cache check: ${chalk.red(err.message)}`
    }).catch(() => process.exit(1));

    if(duplicate){
        return { packageId: duplicate.packageId };
    }

    const input = {
        workspaceId: workspace.id,
        provider: 'remote',
        scope: buildContext.scope,
        listingChecksum: hash,
        manifest: JSON.stringify(manifest)
    }

    const task = await oraPromise(clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceBuildTasksClient).create(ctx.token, workspace.id, input)), {
        text: 'Starting a build process for the package',
        successText: res => `Build process is successfully initiated with reference ${chalk.bold(res.id)}`,
        failText: err => `Could not initiate the build process: ${chalk.red(err.message)}`
    });

    const zip = await createZip(files);

    const uploadData = new FormData()
    uploadData.append('file', await fileFromPath(zip))

    const uploaded = await oraPromise(clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceBuildTasksClient).uploadBundleById(ctx.token, workspace.id, task.id, uploadData)), {
        text: 'Uploading the package bundle to build',
        successText: res => `The package was successfully built with reference ${chalk.bold(res.packageId)}`,
        failText: err => `Could not upload and build the package: ${chalk.red(err.message)}`
    });

    if(uploaded.status === 'failed'){
        ora().fail(`The package build failed: ${uploaded.message} (${uploaded.errorCode})`)
        process.exit(1);
    }

    return { packageId: uploaded.packageId }
}

async function tryCreateFromCache(context: ResolvedContext, input: any): Promise<{ packageId: string } | null>{

    try {
        const result = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspacePackagesClient).fromCache(ctx.token, input.workspaceId, input));
        return { packageId: result.id }
    }
    catch(e){

        if(e instanceof ApiError && e.kind === 'not_found'){
            return null;
        }
    }

    return null;
}