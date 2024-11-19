import { ResolvedContext } from "@/core/types";
import { shocClient } from "@/clients/shoc";
import clientGuard from "@/services/client-guard";
import UserWorkspacesClient from "@/clients/shoc/workspace/user-workspaces-client";
import chalk from "chalk";
import { requireSession } from "@/services/session-service";
import ora, { oraPromise } from "ora";
import { RunContext } from "./types";
import build from "../build-service";
import { getGitDetails } from "../git";

export default async function run(context: ResolvedContext, runContext: RunContext) : Promise<{ }> {
    
    const session = await oraPromise(requireSession(context.providerUrl.toString()), {
        successText: res => `🔑 Authenticated by ${chalk.bold(res.name)} at ${chalk.bold(runContext.workspace)}`,
        failText: err => `Could not authenticate: ${chalk.red(err.message)}` 
    }).catch(() => process.exit(1));

    const workspace = await oraPromise(clientGuard(context, (ctx) => shocClient(ctx.apiRoot, UserWorkspacesClient).getByName(ctx.token, runContext.workspace)), {
        text: `Validating workspace ${chalk.bold(runContext.workspace)}`,
        successText: res => `🌎 Workspace ${chalk.bold(res.name)} is valid`,
        failText: err => `The workspace ${chalk.bold(runContext.workspace)} could not be found: ${chalk.red(err.message)}`
    }).catch(() => process.exit(1));


    const gitResult = await getGitDetails(runContext.dir);
    console.log("Git details", gitResult)

    // const buildResult = await build(context, {
    //     workspace: runContext.workspace,
    //     dir: runContext.dir,
    //     buildFile: runContext.buildFile,
    //     scope: runContext.scope,
    //     workspaceReference: { id: workspace.id },
    //     session: session
    // })

    return { }
}
