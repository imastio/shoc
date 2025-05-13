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
    const [data, setData] = useState(initialValue);
    const [errors, setErrors] = useState<any[]>([]);

    const load = useCallback(async () => {
        setProgress(true);

        const { data, errors } = await rpc('cluster/workspace-clusters/getByName', { name: initialValue.name })

        if (errors) {
            setErrors(errors);
            setData(null);
        } else {
            setErrors([])
            setData(data)
        }

        setProgress(false);

    }, [initialValue.name])

    const value = useMemo(() => ({
        initialValue: mapper(initialValue),
        value: mapper(data ?? initialValue),
        load,
        loading: progress,
        errors: errors
    }), [data, initialValue, load, progress, errors])
    
    return <ClusterContext.Provider value={value}>
        {children}
    </ClusterContext.Provider>
}