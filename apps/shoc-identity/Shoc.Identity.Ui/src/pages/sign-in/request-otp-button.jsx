import { authClient, clientGuard } from "@/clients";
import Icons from "@/components/generic/icons";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";
import useAuthorizeContext from "@/hooks/use-authorize-context";
import { cn } from "@/lib/utils";
import { useCallback, useEffect, useState } from "react"
import { useCountdown } from "usehooks-ts";

const EMAIL_REGEX = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;;

export default function RequestOtpButton({ email }) {
    const { toast } = useToast();
    const [progress, setProgress] = useState(false);
    const [sent, setSent] = useState(false);
    const authorizeContext = useAuthorizeContext();
    const [count, { startCountdown, stopCountdown, resetCountdown }] = useCountdown({
        countStart: 60,
        intervalMs: 1000,
    });

    useEffect(() => {
        if (count === 0) {
            setSent(false);
            resetCountdown();
        }
    }, [count, resetCountdown])

    const isValid = EMAIL_REGEX.test(email);

    const requestOtp = useCallback(async () => {

        setProgress(true);

        const result = await clientGuard(() => authClient.requestOtp({
            email,
            returnUrl: authorizeContext.returnUrl,
            signinMethod: "otp",
            deliveryMethod: "email"
        }));

        setProgress(false);

        toast({
            title: 'One-time password sent',
            description: 'Please check your email to get your one-time password!',
            duration: 5000
        });
        startCountdown();
        setSent(true);

    }, [email, authorizeContext.returnUrl, toast, startCountdown]);

    return <Button
        className={cn(isValid ? '' : 'hidden')}
        variant="outline"
        type="button"
        title="Enter a valid email to request one-time password!"
        disabled={progress || !isValid || sent}
        onClick={() => requestOtp()}
    >
        <Icons.spinner className={cn("mr-2", "h-4", "w-4", "animate-spin", progress ? "" : "hidden")} />
        <Icons.otp className={cn("mr-2", "h-4", "w-4", progress ? "hidden" : "")} />
        {" "}
        One-time password {(sent && count > 0) ? `(${count}s)` : ''}
    </Button>

}