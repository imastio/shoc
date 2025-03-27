import { authClient, clientGuard } from "@/clients";
import OidcContext from "./oidc-context";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import useAuthorizeContext from "@/hooks/use-authorize-context";

export default function OidcProvider({ children }) {

    const [ctx, setCtx] = useState<any>({});
    const [progress, setProgress] = useState(true);
    const { returnUrl }: any = useAuthorizeContext();
    const initiatlized = useRef(false);
    
    const load = useCallback(async (returnUrl: string) => {
        setProgress(true);

        const result = await clientGuard(() => authClient.signinContext({
            returnUrl
        }));
        
        setProgress(false);
        
        initiatlized.current = true;

        if (result.error) {
            setCtx({})
            return;
        }

        setCtx(result.payload || {});

    }, []);

    useEffect(() => {
        if(returnUrl){
            load(returnUrl);
        }
    }, [load, returnUrl])

    const value = useMemo(() => ({
        lang: ctx.lang,
        loginHint: ctx.loginHint ?? '',
        progress: initiatlized.current ? false : progress
    }), [ctx, progress])

    return <OidcContext.Provider value={value}>
        {children}
    </OidcContext.Provider>
}