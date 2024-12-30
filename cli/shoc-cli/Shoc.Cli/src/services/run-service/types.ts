export interface RunContext {
    workspace: string,
    dir: string,
    buildFile: string,
    scope: string,
    runFile: string
}

export interface RunManifestArray {
    replicas: number | null | undefined,
    indexer: string | null | undefined,
    counter: string | null | undefined
}

export interface RunManifestEnv {
    use: string[] | null | undefined,
    override: Record<string, string> | null | undefined
}

export interface ParsedRunManifest {
    kind?: string,
    labels?: string[],
    cluster: string,
    args: string[] | null | undefined,
    array: RunManifestArray | null | undefined,
    env: RunManifestEnv | null | undefined
}

export interface ParsedRunManifestResult {
    manifest: ParsedRunManifest,
    runFile: string,
}

export interface RunInitializationResult {
    gitRepoId?: string | null,
    labelIds?: string[] | null,
}