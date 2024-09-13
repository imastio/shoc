"use client"

import { useEffect } from "react";
import { useCallback, useState } from "react";
import AccessContext from "./access-context";
import { useMemo } from "react";
import { useApiAuthentication } from "../api-authentication/use-api-authentication";
import { useSession } from "next-auth/react";
import { selfClient } from "@/clients/shoc";
import CurrentUserClient from "@/clients/shoc/identity/current-user-client";

export default function AccessProvider({ children } : { children: React.ReactNode }){
    
    const session = useSession();
    const subject = session.data?.user?.id;

    const { withToken, ready } = useApiAuthentication();
    const [accesses, setAccesses] = useState<Set<string>>(new Set<string>([]));
    const [progress, setProgress] = useState(true);

    const hasAny = useCallback((requirements: string[]) => {
        return requirements.some(req => accesses.has(req));
    }, [accesses]);

    const hasAll = useCallback((requirements: string[]) => {
        return requirements.every(req => accesses.has(req));
    }, [accesses]);

    const load = useCallback(async () => {
        
        setProgress(true);
        const result = await withToken((token: string) => selfClient(CurrentUserClient).getEffectiveAccesses(token));
        
        setProgress(false);

        if (result.error) {
            setAccesses(new Set<string>([]))
            return;
        }

        setAccesses(new Set<string>((result.payload || [])));

    }, [withToken]);

    useEffect(() => {

        if(!subject || !ready){
            return;
        }

        load();
    }, [load, subject, ready]);


    const value = useMemo(() => ({
        accesses,
        progress,
        hasAny,
        hasAll
    }), [accesses, progress, hasAny, hasAll])


    return <AccessContext.Provider value={value}>
        {children}
    </AccessContext.Provider>
}