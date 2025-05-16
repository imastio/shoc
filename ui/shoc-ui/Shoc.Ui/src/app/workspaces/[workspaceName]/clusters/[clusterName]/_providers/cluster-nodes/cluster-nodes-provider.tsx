"use client"

import { useCallback, useEffect, useMemo, useState } from "react";
import { rpc } from "@/server-actions/rpc";
import ClusterNodesContext from "./cluster-nodes-context";
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

    const value = useMemo(() => ({
        value: data,
        load,
        loading: progress,
        errors: errors
    }), [data, load, progress, errors])
    
    return <ClusterNodesContext.Provider value={value}>
        {children}
    </ClusterNodesContext.Provider>
}