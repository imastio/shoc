"use client"

import { useCallback, useMemo, useState } from "react";
import ClusterConnectivityContext, { ClusterConnectivityValueType } from "./cluster-connectivity-context";
import { rpc } from "@/server-actions/rpc";

const mapper = (value: any): ClusterConnectivityValueType => {
    return {
        id: value.id,
        workspaceId: value.workspaceId,
        configured: value.configured,
        connected: value.connected,
        message: value.message,
        nodesCount: value.nodesCount,
        updated: value.updated
    }
}

export default function ClusterConnectivityProvider({ children, initialValue }: { children: React.ReactNode, initialValue: any }) {

    const [progress, setProgress] = useState<boolean>(false)
    const [data, setData] = useState(initialValue);
    const [errors, setErrors] = useState<any[]>([]);

    const load = useCallback(async () => {
        setProgress(true);

        const { data, errors } = await rpc('cluster/workspace-cluster-instance/getConnectivityById', { workspaceId: initialValue.workspaceId, id: initialValue.id })

        if (errors) {
            setErrors(errors);
            setData(null);
        } else {
            setErrors([])
            setData(data)
        }

        setProgress(false);

    }, [initialValue.workspaceId, initialValue.id])

    const value = useMemo(() => ({
        initialValue: mapper(initialValue),
        value: mapper(data ?? initialValue),
        load,
        loading: progress,
        errors: errors
    }), [data, initialValue, load, progress, errors])
    
    return <ClusterConnectivityContext.Provider value={value}>
        {children}
    </ClusterConnectivityContext.Provider>
}