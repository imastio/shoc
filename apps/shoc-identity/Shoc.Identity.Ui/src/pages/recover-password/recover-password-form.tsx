import Icons from "@/components/generic/icons"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { useCallback, useState } from "react"
import { useSearchParams } from "react-router-dom"
import useAuthorizeContext from "@/hooks/use-authorize-context"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import useNavigateExt from "@/hooks/use-navigate-ext"
import { authClient, clientGuard } from "@/clients"
import ErrorAlert from "@/components/generic/error-alert"
import { useIntl } from "react-intl";
import { validateEmail } from "@/lib/validation"
import RequestRecoveryButton from "./request-recovery-button"
import useOidc from "@/providers/oidc-provider/use-oidc"

const MIN_PASSWORD_LENGTH = 6;
const MIN_CODE_LENGTH = 6;

export default function RecoverPasswordForm() {
    const [searchParams] = useSearchParams();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);
    const intl = useIntl();
    const authorizeContext = useAuthorizeContext();
    const oidc = useOidc();
    const navigateExt = useNavigateExt();

    const formSchema = z.object({
        email: z.string().email(intl.formatMessage({id: 'auth.validation.email'})),
        code: z.string().min(MIN_CODE_LENGTH, intl.formatMessage({id: 'auth.validation.recoveryCode'}, {num: MIN_CODE_LENGTH})),
        password: z.string().min(MIN_CODE_LENGTH, intl.formatMessage({id: 'auth.validation.password'}, {num: MIN_PASSWORD_LENGTH})),
        passwordConfirmation: z.string().min(MIN_CODE_LENGTH, intl.formatMessage({id: 'auth.validation.password'}, {num: MIN_PASSWORD_LENGTH}))
    }).refine((data) => data.password === data.passwordConfirmation, {
        message: intl.formatMessage({id: 'auth.validation.passwordConfirmation'}),
        path: ["passwordConfirmation"],
    });

    const form = useForm({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: oidc.loginHint,
            code: '',
            password: '',
            passwordConfirmation: ''

        },
        shouldUseNativeValidation: false
    })

    const email = form.watch('email');
    const code = form.watch('code');

    const emailValid = validateEmail(email);
    const codeValid = code.length >= MIN_CODE_LENGTH;

    const recover = useCallback(async ({ email, code, password, returnUrl }) => {

        setProgress(true);
        setErrors([]);

        const result = await clientGuard(() => authClient.processPasswordRecovery({
            email,
            code,
            password,
            returnUrl,
            lang: intl.locale
        }));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload?.errors || []);
            return;
        }

        navigateExt({
            pathname: "/sign-in",
            search: `?${searchParams.toString()}`,
            searchOverrides: { login_hint: email }
        })

    }, [intl]);

    async function onSubmit(values) {
        await recover({
            email: values.email,
            code: values.code,
            password: values.password,
            returnUrl: authorizeContext.returnUrl
        });
    }

    return (
        <>
            <ErrorAlert errors={errors} title={intl.formatMessage({ id: 'auth.recover.unable' })} />
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
                                        <FormDescription>
                                            {intl.formatMessage({id: 'auth.recover.emailDescription'})}
                                        </FormDescription>
                                    </FormItem>
                                )}
                            />
                        </div>
                        <div className={cn("grid gap-1", emailValid ? "" : "hidden")}>
                            <FormField
                                control={form.control}
                                name="code"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Code</FormLabel>
                                        <FormControl>
                                            <Input
                                                placeholder={intl.formatMessage({id: 'auth.placeholders.recoveryCode'})}
                                                type="text"
                                                autoComplete="off"
                                                aria-autocomplete="none"
                                                disabled={progress || !emailValid}
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                        <FormDescription>
                                        {intl.formatMessage({id: 'auth.recover.codeDescription'})}
                                        </FormDescription>
                                    </FormItem>
                                )}
                            />
                        </div>
                        <div className={cn("grid gap-1", emailValid && codeValid ? "" : "hidden")}>
                            <FormField
                                control={form.control}
                                name="password"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>{intl.formatMessage({id: 'auth.labels.newPassword'})}</FormLabel>
                                        <FormControl>
                                            <Input
                                                placeholder="**********"
                                                type="password"
                                                autoComplete="off"
                                                aria-autocomplete="none"
                                                disabled={progress || !emailValid || !codeValid}
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>
                        <div className={cn("grid gap-1", emailValid && codeValid ? "" : "hidden")}>
                            <FormField
                                control={form.control}
                                name="passwordConfirmation"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>{intl.formatMessage({id: 'auth.labels.passwordConfirmation'})}</FormLabel>
                                        <FormControl>
                                            <Input
                                                placeholder="**********"
                                                type="password"
                                                autoComplete="off"
                                                aria-autocomplete="none"
                                                disabled={progress || !emailValid || !codeValid}
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>
                        <RequestRecoveryButton email={email} />
                        <Button type="submit" disabled={progress || !emailValid || !codeValid} className="mt-2">
                            {progress && (
                                <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
                            )}
                            {intl.formatMessage({id: 'auth.common.continue'})}
                        </Button>
                    </div>
                </form>
            </Form>
        </>
    )
}