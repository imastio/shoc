export interface BuildManifest {
    template: string,
    spec: any,
    ignore: string[] | undefined
}

export interface FileEntry {
    fullPath: string,
    modified: bigint | number,
    size: bigint | number
}

export interface BuildFileEntry extends FileEntry{
    localPath: string
}

export interface BuildManifestResult {
    manifest: BuildManifest,
    buildFile: FileEntry
}

export interface BuildListing {
    files: BuildFileEntry[]
}

export interface BuildContext {
    workspace: string,
    dir: string,
    buildFile: string,
    scope: string
}