"use client"

import { useCallback, useEffect, useMemo, useState } from "react";
import { rpc } from "@/server-actions/rpc";
import ClusterNodesContext from "./cluster-nodes-context";


export default function ClusterNodesProvider({ workspaceId, id, preload, children }: { workspaceId: string, id: string, preload: boolean, children: React.ReactNode }) {

    const [progress, setProgress] = useState<boolean>(false)
    const [data, setData] = useState(null);
    const [errors, setErrors] = useState<any[]>([]);

    const load = useCallback(async () => {
        setProgress(true);

        const { data, errors } = await rpc('cluster/workspace-cluster-instance/getNodesById', { workspaceId, id })

        if (errors) {
            setErrors(errors);
            setData(null);
        } else {
            setErrors([])
            setData(data)
        }

        setProgress(false);

    }, [workspaceId, id])

    useEffect(() => {
        if(preload){
            load()
        }
    }, [load, preload])

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