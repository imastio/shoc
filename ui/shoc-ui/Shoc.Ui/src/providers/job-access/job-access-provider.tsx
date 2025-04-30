"use client"

import { useCallback } from "react";
import { useMemo } from "react";
import JobAccessContext from "./job-access-context";

export default function JobAccessProvider({ children, permissions = [] } : { children: React.ReactNode, permissions: string[] }){
    
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

    return <JobAccessContext.Provider value={value}>
        {children}
    </JobAccessContext.Provider>
}