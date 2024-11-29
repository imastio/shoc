export interface RunContext {
    workspace: string,
    dir: string,
    buildFile: string,
    scope: string,
    runFile: string
}

export interface RunManifest {
    kind?: string,
    labels?: string[],
    cluster: string,
}

export interface RunManifestResult {
    manifest: RunManifest,
    runFile: string,
}

export interface RunInitializationResult {
    gitRepoId?: string | null,
    labelIds?: string[] | null,
}