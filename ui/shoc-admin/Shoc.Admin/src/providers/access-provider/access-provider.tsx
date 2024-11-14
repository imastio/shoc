"use client"

import { useCallback } from "react";
import AccessContext from "./access-context";
import { useMemo } from "react";

export default function AccessProvider({ accesses, children } : { accesses: Set<string>, children: React.ReactNode }){

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


    return <AccessContext.Provider value={value}>
        {children}
    </AccessContext.Provider>
}