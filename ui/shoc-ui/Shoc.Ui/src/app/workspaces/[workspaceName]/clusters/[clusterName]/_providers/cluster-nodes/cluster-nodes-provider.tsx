"use client"

import { useCallback, useEffect, useMemo, useState } from "react";
import { rpc } from "@/server-actions/rpc";
import ClusterNodesContext, { ClusterNodesSummary, ClusterNodeValueType, NodeResourceDescriptor } from "./cluster-nodes-context";
import useClusterConnectivity from "@/providers/cluster-connectivity/use-cluster-connectivity";

export default function ClusterNodesProvider({  children }: { children: React.ReactNode }) {

    const { value: connectivity } = useClusterConnectivity();
    const [progress, setProgress] = useState<boolean>(connectivity?.connected)
    const [data, setData] = useState(null);
    const [errors, setErrors] = useState<any[]>([]);

    const load = useCallback(async () => {
        setProgress(true);

        const { data, errors } = await rpc('cluster/workspace-cluster-instance/getNodesById', { 
            workspaceId: connectivity.workspaceId,
            id: connectivity.id
        })

        if (errors) {
            setErrors(errors);
            setData(null);
        } else {
            setErrors([])
            setData(data)
        }

        setProgress(false);

    }, [connectivity.workspaceId, connectivity.id])

    useEffect(() => {
        if(connectivity?.connected){
            load()
        }
    }, [load, connectivity.connected])

    
    const summary: ClusterNodesSummary | null = useMemo(() => {
        
        const nodes = data as ClusterNodeValueType[] | null;
        if(!nodes){
            return null
        }

        const cpu: NodeResourceDescriptor = {
            allocatable: nodes.map(n => n.allocatableCpu).reduce((p, c) => p + c, 0),
            capacity: nodes.map(n => n.capacityCpu).reduce((p, c) => p + c, 0),
            used: nodes.map(n => n.usedCpu ?? 0).reduce((p, c) => p  + c, 0)
        }

        const memory: NodeResourceDescriptor = {
            allocatable: nodes.map(n => n.allocatableMemory).reduce((p, c) => p + c, 0),
            capacity: nodes.map(n => n.capacityMemory).reduce((p, c) => p + c, 0),
            used: nodes.map(n => n.usedMemory ?? 0).reduce((p, c) => p  + c, 0)
        }

        const nvidiaGpu: NodeResourceDescriptor = {
            allocatable: nodes.map(n => n.allocatableNvidiaGpu).reduce((p, c) => p + c, 0),
            capacity: nodes.map(n => n.capacityNvidiaGpu ?? 0).reduce((p, c) => p + c, 0),
            used: undefined
        }

        const amdGpu: NodeResourceDescriptor = {
            allocatable: nodes.map(n => n.allocatableAmdGpu).reduce((p, c) => p + c, 0),
            capacity: nodes.map(n => n.capacityAmdGpu ?? 0).reduce((p, c) => p + c, 0),
            used: undefined
        }

        return {
            cpu,
            memory,
            nvidiaGpu,
            amdGpu
        }

    }, [data])

    const value = useMemo(() => ({
        value: data,
        summary,
        load,
        loading: progress,
        errors: errors
    }), [data, load, progress, errors])

    
    return <ClusterNodesContext.Provider value={value}>
        {children}
    </ClusterNodesContext.Provider>
}