"use client"

import { useCallback, useMemo, useState } from "react";
import ClusterContext, { ClusterValueType } from "./cluster-context";
import { rpc } from "@/server-actions/rpc";

const mapper = (value: any): ClusterValueType => {
    return {
        id: value.id,
        workspaceId: value.workspaceId,
        workspaceName: value.workspaceName,
        name: value.name,
        description: value.description,
        type: value.type,
        status: value.status,
        created: value.created,
        updated: value.updated
    }
}

export default function ClusterProvider({ children, initialValue }: { children: React.ReactNode, initialValue: any }) {

    const [progress, setProgress] = useState<boolean>(false)
    const [result, setResult] = useState<{ data: any, errors: any[] }>({ data: initialValue, errors: [] })

    const load = useCallback(async () => {
        setProgress(true);

        const { data, errors } = await rpc('cluster/workspace-clusters/getByName', { workspaceId: initialValue.workspaceId, name: initialValue.name })

        if (errors) {
            setResult({ data: null, errors })
        } else {
            setResult({ data: data, errors: [] })
        }

        setProgress(false);

    }, [initialValue.workspaceId, initialValue.name])

    const initialValueMapped = useMemo(() => mapper(initialValue), [initialValue]);
    const valueMapped = useMemo(() => mapper(result.data ?? initialValue), [result.data, initialValue])

    const value = useMemo(() => ({
        initialValue: initialValueMapped,
        value: valueMapped,
        load,
        loading: progress,
        errors: result.errors
    }), [initialValueMapped, valueMapped, load, progress, result.errors])
    
    return <ClusterContext.Provider value={value}>
        {children}
    </ClusterContext.Provider>
}