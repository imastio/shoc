import { ResolvedContext } from "@/core/types";
import { shocClient } from "@/clients/shoc";
import clientGuard from "@/services/client-guard";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import chalk from "chalk";
import { getAuthenticatedContext } from "@/services/session-service";
import { oraPromise } from "ora";
import { RunContext } from "./types";
import build from "../build-service";
import { getRunManifest, initialize } from "./implementation";
import WorkspaceClustersClient from "@/clients/shoc/cluster/workspace-clusters-client";
import WorkspaceJobsClient from "@/clients/shoc/job/workspace-jobs-client";
import { mapArgs, mapArray, mapEnv, mapResources, mapSpec } from "./mappers";

export default async function run(context: ResolvedContext, runContext: RunContext) : Promise<{ }> {
    
    const authenticatedContext = await oraPromise(getAuthenticatedContext(context.providerUrl), {
        text: 'Validating user authentication',
        successText: res => `🔑 Authenticated by ${chalk.bold(res.session.name)} at ${chalk.bold(runContext.workspace)}`,
        failText: err => `Could not authenticate: ${chalk.red(err.message)}` 
    }).catch(() => process.exit(1));

    const { id: workspaceId } = await oraPromise(clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, runContext.workspace)), {
        text: `Validating workspace ${chalk.bold(runContext.workspace)}`,
        successText: res => `🌎 Workspace ${chalk.bold(res.name)} is valid`,
        failText: err => `The workspace ${chalk.bold(runContext.workspace)} is not valid: ${chalk.red(err.message)}`
    }).catch(() => process.exit(1));

    const { manifest } = await oraPromise(getRunManifest(runContext), {
        successText: res => `📄 Detected build manifest at ${chalk.bold(res.runFile)}`,
        failText: err => `Build manifest could not be found: ${chalk.red(err.message)}` 
    }).catch(() => process.exit(1));

    const { gitRepoId, labelIds } = await oraPromise(initialize(context, runContext, workspaceId, manifest), {
        successText: res => `ℹ️  Initialization completed successfully`,
        failText: err => `Initialization failed: ${chalk.red(err.message)}` 
    }).catch(() => process.exit(1));

    const { packageId } = await build(context, {
        workspace: runContext.workspace,
        dir: runContext.dir,
        buildFile: runContext.buildFile,
        scope: runContext.scope,
        workspaceReference: { id: workspaceId },
        authenticatedContext: authenticatedContext
    })

    const { id: clusterId } = await oraPromise(clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceClustersClient).getByName(ctx.token, workspaceId, manifest.cluster)), {
        text: `Ensuring cluster ${chalk.bold(manifest.cluster)} exists`,
        successText: res => `🔌 Cluster ${chalk.bold(res.name)} exists`,
        failText: err => `The cluster ${chalk.bold(manifest.cluster)} could not be found: ${chalk.red(err.message)}`
    }).catch(() => process.exit(1));

    const input = {
        workspaceId,
        scope: runContext.scope,
        manifest: {
            gitRepoId: gitRepoId,
            labelIds: labelIds ?? [],
            clusterId,
            packageId,
            args: mapArgs(manifest.args),
            array: mapArray(manifest.array),
            env: mapEnv(manifest.env),
            resources: mapResources(manifest.resources),
            spec: mapSpec(manifest.spec)
        }
    }
   
    const job = await oraPromise(clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceJobsClient).create(ctx.token, workspaceId, input)), {
        text: 'Initializing a new job in the system',
        successText: res => `⌛ Job (${chalk.bold(res.localId)}) in workspace ${chalk.bold(runContext.workspace)} was successfully created`,
        failText: err => `Could not initialize a job with the given manifest: ${chalk.red(err.message)}`
    });

    return { }
}
