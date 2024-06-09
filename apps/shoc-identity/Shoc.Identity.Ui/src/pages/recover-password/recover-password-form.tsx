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
import useNavigateExt from "@/hooks/auth/use-navigate-ext"
import { authClient, clientGuard } from "@/clients"
import ErrorAlert from "@/components/generic/error-alert"
import { useIntl } from "react-intl";
import { validateEmail } from "@/lib/validation"
import RequestRecoveryButton from "./request-recovery-button"

export default function RecoverPasswordForm() {
    const [searchParams] = useSearchParams();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);
    const intl = useIntl();
    const authorizeContext = useAuthorizeContext();
    const navigateExt = useNavigateExt();

    const formSchema = z.object({
        email: z.string().email('Enter a valid email!'),
        code: z.string().min(6, 'The code have at least 6 characters!'),
        password: z.string().min(6, 'Password must have at least 6 characters!'),
        passwordConfirmation: z.string().min(6, 'Password must have at least 6 characters!')
    }).refine((data) => data.password === data.passwordConfirmation, {
        message: "Passwords don't match",
        path: ["passwordConfirmation"],
    });

    const form = useForm({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: authorizeContext.loginHint,
            code: '',
            password: '',
            passwordConfirmation: ''

        },
        shouldUseNativeValidation: false
    })

    const email = form.watch('email');
    const code = form.watch('code');

    const emailValid = validateEmail(email);
    const codeValid = code.length >= 6;

    const recover = useCallback(async ({ email, code, password }) => {

        setProgress(true);
        setErrors([]);

        const result = await clientGuard(() => authClient.processPasswordRecovery({
            email,
            code,
            password
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

    }, []);

    async function onSubmit(values) {
        await recover({
            email: values.email,
            code: values.code,
            password: values.password
        });
    }

    return (
        <>
            <ErrorAlert errors={errors} title={intl.formatMessage({ id: 'auth.confirm.unable' })} />
            <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)}>
                    <div className="grid gap-2">
                        <div className="grid gap-1">
                            <FormField
                                control={form.control}
                                name="email"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Email</FormLabel>
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
                                            We will send you a confirmation code to this email.
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
                                                placeholder="Your confirmation code"
                                                type="text"
                                                autoComplete="off"
                                                aria-autocomplete="none"
                                                disabled={progress || !emailValid}
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                        <FormDescription>
                                            The code you've recieved to your email.
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
                                        <FormLabel>New Password</FormLabel>
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
                                        <FormLabel>Password Confirmation</FormLabel>
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
                            Continue
                        </Button>

                    </div>
                </form>
            </Form>
        </>
    )
}