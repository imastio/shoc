import { useMemo } from "react";
import { useSearchParams } from "react-router-dom";

export default function useAuthorizeContext(){
    const [searchParams] = useSearchParams();

    return useMemo(() => ({
        returnUrl: searchParams.get('return_url') || searchParams.get('returnUrl') || '/',
        loginHint: searchParams.get('login_hint') || searchParams.get('loginHint') || ''
    }), [searchParams])
}