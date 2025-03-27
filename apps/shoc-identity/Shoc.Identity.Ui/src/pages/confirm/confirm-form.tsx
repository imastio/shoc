import Icons from "@/components/generic/icons"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { useCallback, useState } from "react"
import useAuthorizeContext from "@/hooks/use-authorize-context"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import { authClient, clientGuard } from "@/clients"
import ErrorAlert from "@/components/generic/error-alert"
import { useIntl } from "react-intl";
import RequestConfirmationButton from "./request-confirmation-button"
import useOidc from "@/providers/oidc-provider/use-oidc"

const MIN_CODE_LENGTH = 6;

export default function ConfirmForm() {
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);
    const intl = useIntl();
    const authorizeContext = useAuthorizeContext();
    const oidc = useOidc();

    const formSchema = z.object({
        email: z.string().email(intl.formatMessage({id: 'auth.validation.email'})),
        code: z.string().min(MIN_CODE_LENGTH, intl.formatMessage({id: 'auth.validation.confirmationCode'}, {num: MIN_CODE_LENGTH}))
    })

    const form = useForm({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: oidc.loginHint,
            code: ''
        },
        shouldUseNativeValidation: false
    })

    const email = form.watch('email');

    const confirm = useCallback(async ({ email, code, returnUrl }) => {
        
        setProgress(true);
        setErrors([]);

        const result = await clientGuard(() => authClient.processConfirmation({
            target: email,
            targetType: 'email',
            code,
            returnUrl,
            lang: intl.locale
        }));

        
        if(result.error){
            setErrors(result.payload?.errors || []);
            setProgress(false);
            return;
        }
        const payload = result.payload || {};

        const redirectTo = payload.returnUrl || '/';

        window.location.href = redirectTo;

    }, [intl]);

    async function onSubmit(values) {
        await confirm({
            email: values.email, 
            code: values.code, 
            returnUrl: authorizeContext.returnUrl 
        });
    }

    return (
        <>
        <ErrorAlert errors={errors} title={intl.formatMessage({id: 'auth.confirm.unable'})} />
        <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
                <div className="grid gap-2">
                    <div className="grid gap-1">
                        <FormField
                            control={form.control}
                            name="email"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>{intl.formatMessage({id: 'auth.labels.email'})}</FormLabel>
                                    <FormControl>
                                        <Input
                                            autoFocus
                                            placeholder="name@example.com"
                                            type="email"
                                            autoCapitalize="none"
                                            autoComplete="off"
                                            aria-autocomplete="none"
                                            autoCorrect="off"
                                            disabled={progress}
                                            {...field}
                                        />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                    </div>
                    <div className="grid gap-1">
                        <FormField
                            control={form.control}
                            name="code"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>{intl.formatMessage({id: 'auth.labels.code'})}</FormLabel>
                                    <FormControl>
                                        <Input
                                            placeholder={intl.formatMessage({id: 'auth.placeholders.confirmationCode'})}
                                            type="text"
                                            autoComplete="off"
                                            aria-autocomplete="none"
                                            disabled={progress}
                                            {...field}
                                        />
                                    </FormControl>
                                    <FormMessage />
                                    <FormDescription>
                                        {intl.formatMessage({id: 'auth.confirm.codeDescription'})}
                                    </FormDescription>
                                </FormItem>
                            )}
                        />
                    </div>
                    <Button type="submit" disabled={progress} className="mt-2">
                        {progress && (
                            <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
                        )}
                        {intl.formatMessage({id: 'auth.common.continue'})}
                    </Button>
                    <RequestConfirmationButton email={email} />
                </div>
            </form>
        </Form>
        </>
    )
}