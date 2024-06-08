import { authClient, clientGuard } from "@/clients";
import Icons from "@/components/generic/icons";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";
import useAuthorizeContext from "@/hooks/use-authorize-context";
import { cn } from "@/lib/utils";
import { useCallback, useEffect, useState } from "react"
import { useNavigate } from "react-router-dom";
import { useCountdown } from "usehooks-ts";
    
export default function SignOutForm() {
    const authorizeContext = useAuthorizeContext();
    const [progress, setProgress] = useState(authorizeContext.logoutId.length > 0);
    const [manual, setManual] = useState(authorizeContext.logoutId.length === 0);
    const [iframeUrl, setIframeUrl] = useState(null);
    const [redirectUri, setRedirectUri] = useState("/");
    const [continueFlow, setContinueFlow] = useState(true);
    const navigate = useNavigate();

    const signOut = useCallback(async () => {

        setProgress(true);

        const result = await clientGuard(() => authClient.signout({
            requireValidContext: !manual,
            logoutId: authorizeContext.logoutId
        }));


        if (result.error) {
            setManual(true);
            setProgress(false);
            return;
        }

        const payload = result?.payload || {};

        setIframeUrl(payload.signOutIframeUrl);
        setRedirectUri(payload.postLogoutRedirectUri || "/");
        setContinueFlow(payload.continueFlow);

        if (!payload.signOutIframeUrl && payload.postLogoutRedirectUri) {
            window.location.href = payload.postLogoutRedirectUri;
        }
    }, [manual, authorizeContext.logoutId]);

    useEffect(() => {
        if (authorizeContext.logoutId.length > 0) {
            signOut();
        }
    }, [signOut, authorizeContext.logoutId])

    return <>
        <Button
            type="button"
            disabled={progress}
            onClick={() => signOut()}
        >
            <Icons.spinner className={cn("mr-2", "h-4", "w-4", "animate-spin", progress ? "" : "hidden")} />
            <Icons.signOut className={cn("mr-2", "h-4", "w-4", progress ? "hidden" : "")} />
            {" "}
            Continue
        </Button>

        {iframeUrl && <iframe key="signout-notification-iframe"
            title="Sign Out Silently"
            src={iframeUrl}
            style={{ position: 'absolute', width: 0, height: 0, border: 0 }}
            onLoad={() => {
                if (redirectUri.startsWith("/") && !continueFlow) {
                    navigate(redirectUri, { replace: true });
                }
                else {
                    window.location.href = redirectUri;
                }
            }}></iframe>}
    </>
}