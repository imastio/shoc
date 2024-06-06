import { clientGuard, sessionClient } from "@/clients";
import SessionContext from "./session-context";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useInterval, useTimeout } from 'usehooks-ts';

export default function AuthProvider({ children }) {

    const [session, setSession] = useState({});
    const [progress, setProgress] = useState(true);
    const initiatlized = useRef(false);
    const lastSessionId = useRef(undefined);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await clientGuard(() => sessionClient.get());

        setProgress(false);

        initiatlized.current = true;

        if (result.error) {
            setSession({})
            return;
        }

        setSession(result.payload || {});

    }, []);

    useEffect(() => {
        load();
    }, [load])

    useInterval(() => {

        const cookies = Object.fromEntries(document.cookie.split('; ').map(v => v.split(/=(.*)/s).map(decodeURIComponent)));
        const session = cookies['idsrv.session'];

        if (lastSessionId.current !== session) {
            lastSessionId.current = session;
            load();
        }

    }, 2000)


    const value = useMemo(() => ({
        authenticated: session.authenticated || false,
        user: session.authenticated ? {
            id: session.id || null,
            fullName: session.fullName || null,
            email: session.email || null,
            emailVerified: session.emailVerified || null,
        } : null,
        progress: initiatlized.current ? false : progress
    }), [session, progress])

    return <SessionContext.Provider value={value}>
        {children}
    </SessionContext.Provider>
}