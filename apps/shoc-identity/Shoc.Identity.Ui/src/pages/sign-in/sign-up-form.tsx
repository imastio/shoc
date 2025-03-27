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
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import useNavigateExt from "@/hooks/use-navigate-ext"
import { useIntl } from "react-intl"
import { authClient, clientGuard } from "@/clients"
import ErrorAlert from "@/components/generic/error-alert"
import useOidc from "@/providers/oidc-provider/use-oidc"

const MIN_FULL_NAME_LENGTH = 5;
const MIN_PASSWORD_LENGTH = 6

export default function SignUpForm({ className = '', ...props }) {

    const [searchParams] = useSearchParams();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);
    const intl = useIntl();
    const authorizeContext = useAuthorizeContext();
    const navigateExt = useNavigateExt();
    const oidc = useOidc();

    const formSchema = z.object({
        fullName: z.string().min(MIN_FULL_NAME_LENGTH, intl.formatMessage({id: 'auth.validation.fullName'})),
        email: z.string().email(intl.formatMessage({id: 'auth.validation.email'})),
        password: z.string().min(MIN_PASSWORD_LENGTH, intl.formatMessage({id: 'auth.validation.password'}, {num: MIN_PASSWORD_LENGTH})),
        passwordConfirmation: z.string().min(MIN_PASSWORD_LENGTH, intl.formatMessage({id: 'auth.validation.password'}, {num: MIN_PASSWORD_LENGTH}))
    }).refine((data) => data.password === data.passwordConfirmation, {
        message: intl.formatMessage({id: 'auth.validation.passwordConfirmation'}),
        path: ["passwordConfirmation"],
    });
    const form = useForm({
        resolver: zodResolver(formSchema),
        defaultValues: {
            fullName: '',
            email: oidc.loginHint,
            password: '',
            passwordConfirmation: ''
        },
        shouldUseNativeValidation: false
    })

    const signUp = useCallback(async ({ email, password, fullName, returnUrl, lang }) => {
        
        setProgress(true);
        setErrors([]);

        const result = await clientGuard(() => authClient.signup({ email, password, fullName, returnUrl, lang }));
        
        if(result.error){
            setErrors(result.payload?.errors || []);
            setProgress(false);
            return;
        }

        const payload = result.payload || {};

        if (!payload.emailVerified) {
            navigateExt({
                pathname: "/confirm",
                search: `?${searchParams.toString()}`,
                searchOverrides: { login_hint: payload.email }
            })
            return;
        }

        const redirectTo = payload.returnUrl || '/';

        window.location.href = redirectTo;

    }, [navigateExt]);

    async function onSubmit(values) {
        await signUp({ 
            email: values.email, 
            password: values.password, 
            fullName: values.fullName,
            returnUrl: authorizeContext.returnUrl,
            lang: intl.locale
        });
    }

    return (
        <div className={cn("grid gap-6", className)} {...props}>

            <ErrorAlert errors={errors} title={intl.formatMessage({ id: 'auth.signUp.unable' })} />
            <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)}>
                    <div className="grid gap-2">
                    <div className="grid gap-1">
                            <FormField
                                control={form.control}
                                name="fullName"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>{intl.formatMessage({id: 'auth.labels.fullName'})}</FormLabel>
                                        <FormControl>
                                            <Input
                                                autoFocus
                                                placeholder="John Smith"
                                                type="text"
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
                                name="email"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>{intl.formatMessage({id: 'auth.labels.email'})}</FormLabel>
                                        <FormControl>
                                            <Input
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
                        <div className="grid gap-1">
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
                                                disabled={progress}
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>
                        <Button type="submit" disabled={progress}>
                            {progress && (
                                <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
                            )}
                            {intl.formatMessage({id: 'auth.common.continue'})}
                        </Button>
                    </div>
                </form>
            </Form>
        </div>
    )
}