import { useApiAuthentication } from "api-authentication/use-api-authentication";
import { useEffect } from "react";
import { useCallback, useState } from "react";
import AccessContext from "./access-context";
import { useMemo } from "react";
import { useAuth } from "react-oidc-context";
import { currentUserClient } from "api";

export default function AccessProvider({children}){
    
    const auth = useAuth();
    const subject = auth?.user?.profile?.sub;

    const { withToken, ready } = useApiAuthentication();
    const [accesses, setAccesses] = useState(new Set([]));
    const [progress, setProgress] = useState(true);

    const hasAny = useCallback((requirements) => {
        return requirements.some(req => accesses.has(req));
    }, [accesses]);

    const hasAll = useCallback((requirements) => {
        return requirements.every(req => accesses.has(req));
    }, [accesses]);

    const load = useCallback(async () => {
        
        setProgress(true);
        const result = await withToken(token => currentUserClient.getEffectiveAccesses(token));
        
        setProgress(false);

        if (result.error) {
            setAccesses(new Set([]))
            return;
        }

        setAccesses(new Set((result.payload || [])));

    }, [withToken]);

    useEffect(() => {

        if(!subject || !ready){
            return;
        }

        load(subject);
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