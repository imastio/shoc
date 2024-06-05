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

    let prompt = searchParams.get('prompt') || 'login';
    if(!allowedPrompts.includes(prompt)){
        prompt = 'login';
    }

    return useMemo(() => ({
        returnUrl,
        loginHint,
        prompt
    }), [searchParams])
}