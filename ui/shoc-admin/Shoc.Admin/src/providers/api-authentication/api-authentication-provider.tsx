"use client"

import { useCallback, useEffect, useRef, useState } from "react";
import { useMemo } from "react";
import { AxiosError } from "axios";
import { useSession } from "next-auth/react";
import ApiAuthenticationContext from "./api-authentication-context";

type TokenizedFunction = (token: string) => Promise<any>
type TokenizedFunctionWrapper = (fn: TokenizedFunction) => Promise<any>

async function withErrorHandling(fn: () => Promise<any>): Promise<any>  {
    try{
        const result = await fn();
        return { payload: result.data, error: null };
    }
    catch(error){
        if(error instanceof AxiosError && error.response && error.response.data && Array.isArray(error.response.data.errors)){
            return { payload: error.response.data, error }
        }
        return { payload: { errors: [{ code: "UNKNOWN_ERROR", kind: 'unknown'}] }, error };
    }
}

export default function ApiAuthenticationProvider({children}: Readonly<{
    children: React.ReactNode;
  }>){

    const session = useSession();
    const accessToken = useRef('');
    const [ready, setReady] = useState(false);
    
    useEffect(() => {
        accessToken.current = session.status === 'authenticated' && session.data.user ? 'x-authenticated-token' : 'x-no-token';
        setReady(session.status === 'authenticated' && accessToken.current?.length > 0);
    }, [session])

    const withToken: TokenizedFunctionWrapper  = useCallback(async (fn: TokenizedFunction) => {
        return await withErrorHandling(() => fn(accessToken.current))
    }, []);

    const contextValue = useMemo(() => ({
        withToken,
        ready
    }), [withToken, ready])

    return <ApiAuthenticationContext.Provider value={contextValue}>
        {children}
    </ApiAuthenticationContext.Provider>
}