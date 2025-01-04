import { RunManifestArray, RunManifestEnv, RunManifestResources, RunManifestSpec, RunManifestSpecMpi } from "./types";

export function mapArgs(args: string[] | null | undefined){
    return args ?? [];
}

export function mapArray(array: RunManifestArray | null | undefined): any{
    return {
        replicas: array?.replicas ?? null,
        indexer: array?.indexer ?? null,
        counter: array?.counter ?? null
    }
}

export function mapEnv(env: RunManifestEnv | null | undefined): any{
    return {
        use: env?.use ?? [],
        override: env?.override ?? {}
    }
}

export function mapResources(resources: RunManifestResources | null | undefined): any{
    return {
        cpu: asString(resources?.cpu ?? null),
        memory: asString(resources?.memory ?? null),
        nvidiaGpu: asString(resources?.nvidiaGpu ?? null),
        amdGpu: asString(resources?.amdGpu ?? null),
    }
}

export function mapSpecMpi(mpi: RunManifestSpecMpi | null | undefined): any {
    return {
        ...(mpi ?? {})
    }
}

export function mapSpec(spec: RunManifestSpec | null | undefined): any{
    let result = {};

    if(spec?.mpi){
        result = {...result, mpi: mapSpecMpi(spec.mpi)}
    }

    return result
}

function asString(input: any){
    
    if(input === null || input === undefined || typeof input === 'undefined'){
        return null
    }

    if(typeof input === 'bigint' || typeof input === 'number' || typeof input === 'boolean'){
        return String(input);
    }

    if(typeof input === 'string' || typeof input === 'symbol'){
        return input;
    }

    return null;
}
