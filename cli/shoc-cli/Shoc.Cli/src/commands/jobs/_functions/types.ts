export interface BuildObject {
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

export interface BuildObjectResult {
    buildObject: BuildObject,
    buildFile: FileEntry
}

export interface BuildListing {
    files: BuildFileEntry[]
}