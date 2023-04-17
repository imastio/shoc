import { useCallback } from "react";
import { useMemo } from "react";
import { useAuth } from "react-oidc-context"
import { ApiAuthenticationContext } from "./api-authentication-context";

const withErrorHandling = async (fn) => {
    try{
        const result = await fn();
        return { payload: result.data, error: null };
    }
    catch(error){

        if(error && error.response && error.response.data){
            return { payload: error.response.data, error }
        }
        return { payload: { errors: [{ code: "UNKNOWN_ERROR", kind: 1}] }, error };
    }
}

export const ApiAuthenticationProvider = ({children}) => {

    const auth = useAuth();
    const accessToken = auth.user?.access_token;

    const withToken = useCallback(async (fn) => {
        return withErrorHandling(() => fn(accessToken))
    }, [accessToken]);

    const contextValue = useMemo(() => ({
        withToken
    }), [withToken])
    
    return <ApiAuthenticationContext.Provider value={contextValue}>
        {children}
    </ApiAuthenticationContext.Provider>
}