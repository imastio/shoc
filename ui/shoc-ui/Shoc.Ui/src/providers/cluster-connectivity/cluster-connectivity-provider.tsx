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
    const [result, setResult] = useState<{ data: any, errors: any[] }>({ data: initialValue, errors: [] })

    const load = useCallback(async () => {
        setProgress(true);

        const { data, errors } = await rpc('cluster/workspace-cluster-instance/getConnectivityById', { workspaceId: initialValue.workspaceId, id: initialValue.id })

        if (errors) {
            setResult({ data: null, errors })
        } else {
            setResult({ data: data, errors: [] })
        }

        setProgress(false);

    }, [initialValue.workspaceId, initialValue.id])

    const initialValueMapped = useMemo(() => mapper(initialValue), [initialValue]);
    const valueMapped = useMemo(() => mapper(result.data ?? initialValue), [result.data, initialValue])

    const value = useMemo(() => ({
        initialValue: initialValueMapped,
        value: valueMapped,
        load,
        loading: progress,
        errors: result.errors
    }), [valueMapped, initialValueMapped, load, progress, result.errors])
    
    return <ClusterConnectivityContext.Provider value={value}>
        {children}
    </ClusterConnectivityContext.Provider>
}