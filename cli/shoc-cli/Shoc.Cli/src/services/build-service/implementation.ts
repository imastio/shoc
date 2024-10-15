import { DEFAULT_BUILD_FILE } from "@/commands/common";
import { ResolvedContext } from "@/core/types";
import fsDirect from "fs";
import path from "path";
import * as YAML from 'yaml';
import { promises as fs } from 'fs';
import { BuildContext, BuildFileEntry, BuildListing, BuildManifest, BuildManifestResult, FileEntry } from "./types";
import { glob, Path } from "glob";
import crypto from "crypto"
import os from "os";
import archiver from "archiver";

const DEFAULT_IGNORE_PATTERNS = ['**/.git/**', '**/node_modules/**']
const DEFAULT_TEMP_DIR = 'shoc-temp'
const ZIP_FILENAME_SIZE = 16

async function getBuildFile(context: BuildContext): Promise<FileEntry>{
    const targetBuildFile = path.resolve(context.dir, context.buildFile || DEFAULT_BUILD_FILE)

    if (!fsDirect.existsSync(targetBuildFile)) {
        throw Error(`The build file ${targetBuildFile} could not be found!`)
    }

    const stat = await fs.stat(targetBuildFile);

    return {
        fullPath: targetBuildFile,
        modified: stat.mtime.getTime(),
        size: stat.size
    }
}

export async function getBuildManifest(context: BuildContext): Promise<BuildManifestResult> {

    const targetBuildFile = await getBuildFile(context);

    const file = await fs.readFile(targetBuildFile.fullPath, 'utf8');

    return {
        manifest: YAML.parse(file) as BuildManifest,
        buildFile: targetBuildFile
    };
}

export async function getBuildListing(context: BuildContext, manifest: BuildManifest) : Promise<BuildListing>{
    const list: Path[] = await glob(
        '**/*',
        {
            ignore: [...DEFAULT_IGNORE_PATTERNS, ...(manifest.ignore || [])],
            cwd: context.dir,
            nodir: true,
            withFileTypes: true
        }
    );

    const buildEntries: BuildFileEntry[] = [];

    for(const file of list){
        const fileStat = await fs.stat(file.fullpath())
        
        buildEntries.push({
            fullPath: file.fullpath(),
            localPath: file.relative(),
            modified: fileStat.mtime.getTime(),
            size: fileStat.size
        })
    }

    buildEntries.sort((first, second) => first.localPath.localeCompare(second.localPath) )
    
    return {
        files: buildEntries
    }
} 

export function computeListingHash(buildFile: FileEntry, buildEntries: BuildFileEntry[]){

    const records: string[] = [];

    records.push(`__BUILD_FILE__:${buildFile.size}:${buildFile.modified}`);
    
    buildEntries.forEach(entry => records.push(`${entry.localPath}:${entry.size}:${entry.modified}`));

    return crypto.createHash('sha256').update(records.join(',')).digest('hex');
}

function ensureTempDirectory(): string{

    const temp = path.join(os.tmpdir(), DEFAULT_TEMP_DIR);

    if(!fsDirect.existsSync(temp)){
        fsDirect.mkdirSync(temp, { recursive: true })
    }

    return temp;
}

export async function createZip(files: BuildFileEntry[]): Promise<string> {
    return new Promise((resolve, reject) => {
        
        const targetFileName = `${crypto.randomBytes(ZIP_FILENAME_SIZE).toString('hex')}.zip`;
        const targetFilePath = path.join(ensureTempDirectory(), targetFileName);

        const output = fsDirect.createWriteStream(targetFilePath);
        const archive = archiver('zip', { zlib: { level: 9 } });

        output.on('close', () => {
            resolve(targetFilePath);
        });

        archive.on('error', (err: any) => {
            reject(err);
        });

        archive.pipe(output);

        files.forEach(file => {
            archive.file(file.fullPath, { name: file.localPath });
        });

        archive.finalize();
    });
}