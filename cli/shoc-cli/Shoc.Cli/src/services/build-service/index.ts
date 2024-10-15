import { ResolvedContext } from "@/core/types";
import { BuildContext } from "./types";
import { computeListingHash, createZip, getBuildListing, getBuildManifest } from "./implementation";
import { shocClient } from "@/clients/shoc";
import clientGuard from "@/services/client-guard";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import WorkspaceBuildTasksClient from "@/clients/shoc/package/workspace-build-tasks-client";
import { fileFromPath } from "formdata-node/file-from-path";
import { logger } from "../logger";
import chalk from "chalk";
import { requireSession } from "@/services/session-service";
import path from "path";
import ErrorDefinitions from "@/error-handling/error-definitions";
import WorkspacePackagesClient from "@/clients/shoc/package/workspace-packages-client";
import { ApiError } from "@/error-handling/error-types";

export default async function build(context: ResolvedContext, buildContext: BuildContext) : Promise<{ packageId: string }> {
    
    const session = await requireSession(context.providerUrl.toString());

    logger.just(`Build started by ${chalk.green(session.name)} for workspace ${chalk.green(buildContext.workspace)}`);
    logger.just(`Package directory: ${chalk.green(buildContext.dir)}`)
    logger.just(`Package scope: ${chalk.green(buildContext.scope)}`)

    const { buildFile, manifest } = await getBuildManifest(buildContext);

    logger.just(`Build manifest: ${chalk.green(buildFile.fullPath)}`)    

    const { files } = await getBuildListing(buildContext, manifest);

    const hash = computeListingHash(buildFile, files);
    
    logger.just(`Detected ${chalk.green(files.length)} files for bundling`);
    logger.just(`Package checksum: ${chalk.green(hash)}`);

    const workspace = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, buildContext.workspace));

    const duplicate = await tryCreateFromCache(context, {
        workspaceId: workspace.id,
        scope: buildContext.scope,
        listingChecksum: hash
    });

    if(duplicate){
        logger.just(`The package was successfully built from cache: ${chalk.green(duplicate.packageId)}`)
        return { packageId: duplicate.packageId };
    }

    const input = {
        workspaceId: workspace.id,
        provider: 'remote',
        scope: buildContext.scope,
        listingChecksum: hash,
        manifest: JSON.stringify(manifest)
    }

    const task = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceBuildTasksClient).create(ctx.token, workspace.id, input));

    logger.just(`Remote build task initiated with reference ${chalk.green(task.id)}`)

    const zip = await createZip(files);

    logger.just(`Started uploading the bundle for the remote build: ${chalk.green(path.basename(zip))}`)

    const uploadData = new FormData()
    uploadData.append('file', await fileFromPath(zip))

    const uploaded = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceBuildTasksClient).uploadBundleById(ctx.token, workspace.id, task.id, uploadData));

    if(uploaded.status === 'failed'){
        throw ErrorDefinitions.validation(uploaded.message, uploaded.errorCode)
    }

    logger.just(`The package was successfuly built: ${chalk.green(uploaded.packageId)}`);

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