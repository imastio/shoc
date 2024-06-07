import { authClient, clientGuard } from "@/clients";
import Icons from "@/components/generic/icons";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";
import useAuthorizeContext from "@/hooks/use-authorize-context";
import { cn } from "@/lib/utils";
import { useCallback, useEffect, useState } from "react"
import { useCountdown } from "usehooks-ts";

const EMAIL_REGEX = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;

export default function RequestConfirmationButton({ email }) {
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

    const requestConfirmation = useCallback(async () => {

        setProgress(true);

        const result = await clientGuard(() => authClient.requestConfirmation({
            target: email,
            targetType: "email",
            returnUrl: authorizeContext.returnUrl
        }));

        setProgress(false);

        toast({
            title: 'Confirmation code sent',
            description: 'Please check your email to get your confirmation code!',
            duration: 5000
        });
        startCountdown();
        setSent(true);

    }, [email, authorizeContext.returnUrl, toast, startCountdown]);

    return <Button
        variant="outline"
        type="button"
        title="Enter a valid email to request confirmation code!"
        disabled={progress || !isValid || sent}
        onClick={() => requestConfirmation()}
    >
        <Icons.spinner className={cn("mr-2", "h-4", "w-4", "animate-spin", progress ? "" : "hidden")} />
        <Icons.email className={cn("mr-2", "h-4", "w-4", progress ? "hidden" : "")} />
        {" "}
        Request confirmation code {(sent && count > 0) ? `(${count}s)` : ''}
    </Button>

}