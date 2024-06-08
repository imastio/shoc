import { useMemo } from "react";
import { useSearchParams } from "react-router-dom";

const allowedPrompts = ['login', 'create'];

export default function useAuthorizeContext(){
    const [searchParams] = useSearchParams();

    let returnUrl = searchParams.get('return_url') || searchParams.get('returnUrl') || '/';
    if(!returnUrl.startsWith('/')){
        returnUrl = '/'
    }

    let loginHint = searchParams.get('login_hint') || searchParams.get('loginHint') || '';

    let logoutId = searchParams.get('logout_id') || searchParams.get('logoutId') || '';

    let errorId = searchParams.get('error_id') || searchParams.get('errorId') || '';

    let prompt = searchParams.get('prompt') || 'login';
    if(!allowedPrompts.includes(prompt)){
        prompt = 'login';
    }

    return useMemo(() => ({
        returnUrl,
        loginHint,
        prompt,
        logoutId,
        errorId
    }), [searchParams])
}