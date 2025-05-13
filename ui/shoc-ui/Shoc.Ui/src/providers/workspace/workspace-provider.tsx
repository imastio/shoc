"use client"

import { useCallback, useMemo, useState } from "react";
import WorkspaceContext, { WorkspaceValueType } from "./workspace-context";
import { rpc } from "@/server-actions/rpc";

const mapper = (value: any): WorkspaceValueType => {
    return {
        id: value.id,
        name: value.name,
        type: value.type,
        role: value.role
    }
}

export default function WorkspaceProvider({ children, initialValue }: { children: React.ReactNode, initialValue: any }) {

    const [progress, setProgress] = useState<boolean>(false)
    const [data, setData] = useState(initialValue);
    const [errors, setErrors] = useState<any[]>([]);

    const load = useCallback(async () => {
        setProgress(true);

        const { data, errors } = await rpc('workspace/user-workspaces/getByName', { name: initialValue.name })

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

    return <WorkspaceContext.Provider value={value}>
        {children}
    </WorkspaceContext.Provider>
}