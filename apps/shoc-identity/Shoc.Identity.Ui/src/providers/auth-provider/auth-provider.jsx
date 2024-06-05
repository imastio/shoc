import { clientGuard, sessionClient } from "@/clients";
import AuthContext from "./auth-context";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useInterval, useTimeout } from 'usehooks-ts';

export default function AuthProvider({children}){

    const [user, setUser] = useState(null);
    const [progress, setProgress] = useState(true);
    const initiatlized = useRef(false);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await clientGuard(() => sessionClient.get());

        setProgress(false);

        initiatlized.current = true;

        if (result.error) {
            setUser(null)
            return;
        }

        setUser(result.payload || null);

    }, []);

    useEffect(() => {
        load();
    }, [load])


    const value = useMemo(() => ({
        user,
        progress: initiatlized.current ? false : progress,
    }), [user, progress])

    return <AuthContext.Provider value={value}>
        {children}
    </AuthContext.Provider>
}