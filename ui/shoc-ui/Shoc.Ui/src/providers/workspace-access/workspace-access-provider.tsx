"use client"

import { useCallback } from "react";
import { useMemo } from "react";
import WorkspaceAccessContext from "./workspace-access-context";

export default function WorkspaceAccessProvider({ children, permissions = [] } : { children: React.ReactNode, permissions: string[] }){
    
    const accesses = useMemo(() => new Set<string>(permissions), [permissions]);

    const hasAny = useCallback((requirements: string[]) => {
        return requirements.some(req => accesses.has(req));
    }, [accesses]);

    const hasAll = useCallback((requirements: string[]) => {
        return requirements.every(req => accesses.has(req));
    }, [accesses]);

    const value = useMemo(() => ({
        accesses,
        hasAny,
        hasAll
    }), [accesses, hasAny, hasAll])

    return <WorkspaceAccessContext.Provider value={value}>
        {children}
    </WorkspaceAccessContext.Provider>
}