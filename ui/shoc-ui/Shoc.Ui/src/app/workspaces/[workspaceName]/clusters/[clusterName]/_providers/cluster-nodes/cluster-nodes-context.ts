import { createContext } from "react";

const ClusterNodesContext = createContext<ClusterNodesContextValueType | any>({});

export type NodeResourceDescriptor = {
    allocatable: number,
    capacity: number,
    used?: number
}

export type ClusterNodeValueType = {
    name: string,
    labels: Record<string, string>,
    allocatableCpu: number,
    allocatableMemory: number,
    allocatableNvidiaGpu: number,
    allocatableAmdGpu: number,
    capacityCpu: number,
    capacityMemory: number,
    capacityNvidiaGpu: number,
    capacityAmdGpu: number,
    usedCpu?: number,
    usedMemory?: number 
}

export type ClusterNodesSummary = {
    cpu: NodeResourceDescriptor,
    memory: NodeResourceDescriptor,
    nvidiaGpu: NodeResourceDescriptor,
    amdGpu: NodeResourceDescriptor
}

export type ClusterNodesContextValueType = {
    value: ClusterNodeValueType[],
    summary?: ClusterNodesSummary,
    load: () => Promise<any>,
    loading: boolean,
    errors: any[]
}

export default ClusterNodesContext;