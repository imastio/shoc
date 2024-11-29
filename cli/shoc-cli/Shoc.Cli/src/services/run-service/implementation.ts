import { DEFAULT_RUN_FILE } from "@/commands/common";
import WorkspaceGitReposClient from "@/clients/shoc/job/workspace-git-repos-client";
import { getGitDetails, GitDetails } from "../git";
import { RunContext, RunInitializationResult, RunManifest, RunManifestResult } from "./types";
import clientGuard from "../client-guard";
import { ResolvedContext } from "@/core/types";
import { shocClient } from "@/clients/shoc";
import fsDirect from "fs";
import * as YAML from 'yaml';
import { promises as fs } from 'fs';
import path from "path";
import WorkspaceLabelsClient from "@/clients/shoc/job/workspace-labels-client";

export async function getRunManifest(context: RunContext): Promise<RunManifestResult> {

    const targetRunFile = path.resolve(context.dir, context.runFile || DEFAULT_RUN_FILE)

    if (!fsDirect.existsSync(targetRunFile)) {
        throw Error(`The run file ${targetRunFile} could not be found!`)
    }

    const file = await fs.readFile(targetRunFile, 'utf8');

    return {
        manifest: YAML.parse(file) as RunManifest,
        runFile: targetRunFile
    }
}

export async function initialize(context: ResolvedContext, runContext: RunContext, workspaceId: string, manifest: RunManifest): Promise<RunInitializationResult> {

    const gitDetails = await getGitDetails(runContext.dir);

    const gitRepoId = gitDetails ? await tryEnsureGitRepo(context, workspaceId, gitDetails) : null;

    const labelIds = !manifest.labels || manifest.labels.length === 0 ? [] : await ensureLabels(context, workspaceId, manifest.labels)

    return {
        gitRepoId,
        labelIds
    }
}

async function tryEnsureGitRepo(context: ResolvedContext, workspaceId: string, details: GitDetails): Promise<string | null> {

    const input = {
        workspaceId,
        name: details.name,
        owner: details.owner,
        source: details.source,
        repository: details.repo,
        remoteUrl: details.url
    }

    try {
        const result = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceGitReposClient).ensure(ctx.token, workspaceId, input));

        return result.id
    }
    catch(e){
        return null
    }
}


async function ensureLabels(context: ResolvedContext, workspaceId: string, labels: string[]): Promise<string[]> {

    const input = {
        workspaceId,
        names: labels
    }

    try {
        const result = await clientGuard(context, (ctx) => shocClient(ctx.apiRoot, WorkspaceLabelsClient).ensure(ctx.token, workspaceId, input));

        return result.id
    }
    catch(e){
        throw Error(`Could not resolve labels ([${labels.join(', ')}]): ${(e as Error)?.message || 'Unknown error'}`)
    }
}