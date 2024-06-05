import { authClient, clientGuard } from "@/clients"
import { useMemo, useState } from "react";
import authResponseHandler from "./auth-response-handler";

export default function useSignIn(){

    const [progress, setProgress] = useState(false);

    const signIn = useCallback(async ({ email, password, returnUrl }) => {
        
        setProgress(true);

        const result = await clientGuard(() => authClient.signin({ email, password, returnUrl }));

        setProgress(false);
        
        return authResponseHandler(response);
    }, []);

    const value = useMemo(() => ({
        signIn,
        progress
    }), [signIn, progress])
}