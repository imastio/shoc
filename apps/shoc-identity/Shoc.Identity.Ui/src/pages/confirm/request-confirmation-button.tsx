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

export default function RequestConfirmationButton({ email }) {
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

    const requestConfirmation = useCallback(async () => {

        setProgress(true);

        await clientGuard(() => authClient.requestConfirmation({
            target: email,
            targetType: "email",
            returnUrl: authorizeContext.returnUrl,
            lang: intl.locale
        }));

        setProgress(false);

        toast({
            title: intl.formatMessage({id: 'auth.confirm.requestSuccess'}),
            description: intl.formatMessage({id: 'auth.confirm.requestSuccess'}),
            duration: 5000
        });
        startCountdown();
        setSent(true);

    }, [email, authorizeContext.returnUrl, toast, startCountdown, intl]);

    return <Button
        variant="outline"
        type="button"
        title={intl.formatMessage({id: 'auth.confirm.request.hint'})}
        disabled={progress || !isValid || sent}
        onClick={() => requestConfirmation()}
    >
        <Icons.spinner className={cn("mr-2", "h-4", "w-4", "animate-spin", progress ? "" : "hidden")} />
        <Icons.email className={cn("mr-2", "h-4", "w-4", progress ? "hidden" : "")} />
        {" "}
        {intl.formatMessage({id: 'auth.confirm.request.button'})} {(sent && count > 0) ? `(${intl.formatMessage({id: 'auth.common.seconds'}, {seconds: count})})` : ''}
    </Button>

}