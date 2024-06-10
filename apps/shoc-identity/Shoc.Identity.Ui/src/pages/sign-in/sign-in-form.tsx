import Icons from "@/components/generic/icons"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { useCallback, useState } from "react"
import { useSearchParams } from "react-router-dom"
import useAuthorizeContext from "@/hooks/use-authorize-context"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import useNavigateExt from "@/hooks/use-navigate-ext"
import { authClient, clientGuard } from "@/clients"
import ErrorAlert from "@/components/generic/error-alert"
import { useIntl } from "react-intl";
import RequestOtpButton from "./request-otp-button"

const MIN_PASSWORD_LENGTH = 6;

export default function SignInForm() {
    const [searchParams] = useSearchParams();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);
    const intl = useIntl();
    const authorizeContext = useAuthorizeContext();
    const navigateExt = useNavigateExt();

    const formSchema = z.object({
        email: z.string().email(intl.formatMessage({id: 'auth.validation.email'})),
        password: z.string().min(MIN_PASSWORD_LENGTH, intl.formatMessage({id: 'auth.validation.password'}, {num: MIN_PASSWORD_LENGTH}))
    })

    const form = useForm({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: authorizeContext.loginHint,
            password: ''
        },
        shouldUseNativeValidation: false
    })

    const email = form.watch('email');

    const signIn = useCallback(async ({ email, password, returnUrl }) => {
        
        setProgress(true);
        setErrors([]);

        const result = await clientGuard(() => authClient.signin({ email, password, returnUrl, lang: intl.locale }));
    
        if(result.error){
            if (result.payload?.errors?.some(e => e.code === "IDENTITY_UNVERIFIED_EMAIL")) {
                navigateExt({
                  pathname: "/confirm",
                  search: `?${searchParams.toString()}`,
                  searchOverrides: { login_hint: email }
                })
              }

            setErrors(result.payload?.errors || []);
            setProgress(false);
            return;
        }
        const payload = result.payload || {};

        const redirectTo = payload.returnUrl || '/';

        window.location.href = redirectTo;

    }, [navigateExt, intl]);

    async function onSubmit(values) {
        await signIn({ email: values.email, password: values.password, returnUrl: authorizeContext.returnUrl });
    }

    return (
        <>
        <ErrorAlert errors={errors} title={intl.formatMessage({id: 'auth.signIn.unable'})} />
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
                            name="password"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>{intl.formatMessage({id: 'auth.labels.password'})}</FormLabel>
                                    <FormControl>
                                        <Input
                                            placeholder="**********"
                                            type="password"
                                            autoComplete="off"
                                            aria-autocomplete="none"
                                            disabled={progress}
                                            {...field}
                                        />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                    </div>
                    <p className="text-left text-sm text-muted-foreground">
                        <Button className="m-0 p-0" type="button" variant="link" onClick={() => {
                            navigateExt({
                                pathname: "/recover-password",
                                search: `?${searchParams.toString()}`
                            })
                        }}>{intl.formatMessage({id: 'auth.signIn.forgotPassword'})}</Button>
                    </p>
                    <Button type="submit" disabled={progress}>
                        {progress && (
                            <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
                        )}
                        {intl.formatMessage({id: 'auth.common.continue'})}
                    </Button>
                    <RequestOtpButton email={email} />
                </div>
            </form>
        </Form>
        </>
    )
}