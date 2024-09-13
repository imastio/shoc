import { authClient, clientGuard } from "@/clients";
import Icons from "@/components/generic/icons";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";
import useAuthorizeContext from "@/hooks/use-authorize-context";
import { cn } from "@/lib/utils";
import { validateEmail } from "@/lib/validation";
import { useCallback, useEffect, useState } from "react"
import { useIntl } from "react-intl";
import { useCountdown } from "usehooks-ts";

export default function RequestOtpButton({ email }) {
    const { toast } = useToast();
    const [progress, setProgress] = useState(false);
    const [sent, setSent] = useState(false);
    const authorizeContext = useAuthorizeContext();
    const intl = useIntl();
    const [count, { startCountdown, resetCountdown }] = useCountdown({
        countStart: 60,
        intervalMs: 1000,
    });

    useEffect(() => {
        if (count === 0) {
            setSent(false);
            resetCountdown();
        }
    }, [count, resetCountdown])

    const isValid = validateEmail(email);

    const requestOtp = useCallback(async () => {

        setProgress(true);

        await clientGuard(() => authClient.requestOtp({
            email,
            returnUrl: authorizeContext.returnUrl,
            signinMethod: "otp",
            deliveryMethod: "email",
            lang: intl.locale
        }));

        setProgress(false);

        toast({
            title: intl.formatMessage({id: 'auth.otp.requestSuccess'}),
            description: intl.formatMessage({id: 'auth.otp.requestSuccess'}),
            duration: 5000
        });
        startCountdown();
        setSent(true);

    }, [email, authorizeContext.returnUrl, toast, startCountdown, intl]);

    return <Button
        className={cn(isValid ? '' : 'hidden')}
        variant="outline"
        type="button"
        title={intl.formatMessage({id: 'auth.otp.request.hint'})}
        disabled={progress || !isValid || sent}
        onClick={() => requestOtp()}
    >
        <Icons.spinner className={cn("mr-2", "h-4", "w-4", "animate-spin", progress ? "" : "hidden")} />
        <Icons.otp className={cn("mr-2", "h-4", "w-4", progress ? "hidden" : "")} />
        {" "}
        {intl.formatMessage({id: 'auth.otp.request.button'})} {(sent && count > 0) ? `(${intl.formatMessage({id: 'auth.common.seconds'}, {seconds: count})})` : ''}
    </Button>

}