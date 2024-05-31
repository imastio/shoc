import { clientGuard, currentUserClient } from "@/clients";
import AuthContext from "./auth-context";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useInterval, useTimeout } from 'usehooks-ts';

// delay update for 15s
const UPDATE_DELAY_MS = 15000;

export default function AuthProvider({children}){

    const [user, setUser] = useState(null);
    const [progress, setProgress] = useState(true);
    const initiatlized = useRef(false);

    const load = useCallback(async () => {
        setProgress(true);
        await new Promise(resolve => setTimeout(resolve, 2000));
        const result = await clientGuard(() => currentUserClient.get());

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

    useInterval(() => {
        console.log("Entering hool")
        load();
    }, UPDATE_DELAY_MS);

    const value = useMemo(() => ({
        user,
        progress: initiatlized.current ? false : progress,
    }), [user, progress])

    return <AuthContext.Provider value={value}>
        {children}
    </AuthContext.Provider>
}