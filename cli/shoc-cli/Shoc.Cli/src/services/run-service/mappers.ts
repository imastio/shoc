import { RunManifestArray, RunManifestEnv, RunManifestResources, RunManifestSpec, RunManifestSpecMpi } from "./types";

export function mapArgs(args: string[] | null | undefined){
    return args ?? [];
}

export function mapArray(array: RunManifestArray | null | undefined): any{
    return {
        replicas: array?.replicas,
        indexer: array?.indexer,
        counter: array?.counter
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
        cpu: resources?.cpu,
        memory: resources?.memory,
        nvidiaGpu: resources?.nvidiaGpu,
        amdGpu: resources?.amdGpu,
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
