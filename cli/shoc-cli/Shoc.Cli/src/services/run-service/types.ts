export interface RunContext {
    workspace: string,
    dir: string,
    buildFile: string,
    scope: string,
    runFile: string
}

export interface ParsedRunManifest {
    kind?: string,
    labels?: string[],
    cluster: string,
}

export interface ParsedRunManifestResult {
    manifest: ParsedRunManifest,
    runFile: string,
}

export interface RunInitializationResult {
    gitRepoId?: string | null,
    labelIds?: string[] | null,
}