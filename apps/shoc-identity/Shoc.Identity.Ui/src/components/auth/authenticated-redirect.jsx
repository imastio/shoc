import useAuthorizeContext from "@/hooks/use-authorize-context";
import useSession from "@/providers/session-provider/use-session";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

export default function AuthenticatedRedirect(){
    const { authenticated } = useSession();
    const { returnUrl } = useAuthorizeContext();
    const navigate = useNavigate();

    useEffect(() => {

        if(!authenticated){
            return;
        }
        
        if(returnUrl?.startsWith('/connect/authorize/callback')){
            return;
        }

        if(returnUrl?.startsWith('/')){
            navigate(returnUrl, { replace: true })
            return;
        }

        window.location.href = returnUrl || '/';
    }, [authenticated, returnUrl, navigate]);

    return false;
}