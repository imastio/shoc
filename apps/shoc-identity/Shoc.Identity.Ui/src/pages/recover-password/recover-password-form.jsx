import Icons from "@/components/generic/icons"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useCallback, useState } from "react"
import { useSearchParams } from "react-router-dom"
import useNavigateSearch from "@/hooks/use-navigate-search"
import useAuthorizeContext from "@/hooks/use-authorize-context"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import useNavigateExt from "@/hooks/auth/use-navigate-ext"
import { authClient, clientGuard } from "@/clients"
import ErrorAlert from "@/components/generic/error-alert"
import { useIntl } from "react-intl";
import RequestConfirmationButton from "./request-recovery-button"

export default function RecoverPasswordForm() {
    const [searchParams, setSearchParams] = useSearchParams();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);
    const [flowResult, setFlowResult] = useState({});
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

    const recover = useCallback(async ({ email, code, password }) => {
        
        setProgress(true);
        setErrors([]);

        const result = await clientGuard(() => authClient.processPasswordRecovery({
            email,
            code,
            password
        }));

        
        if(result.error){
            setErrors(result.payload?.errors || []);
            return;
        }
        setProgress(false);

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
        <ErrorAlert errors={errors} title={intl.formatMessage({id: 'auth.confirm.unable'})} />
        <Form {...form} autoComplete="off">
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
                                    <FormLabel>Code</FormLabel>
                                    <FormControl>
                                        <Input
                                            placeholder="Your confirmation code"
                                            type="text"
                                            autoComplete="off"
                                            aria-autocomplete="none"
                                            disabled={progress}
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
                    <div className="grid gap-1">
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
                                                disabled={progress || code?.length === 0}
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
                                        <FormLabel>Password Confirmation</FormLabel>
                                        <FormControl>
                                            <Input
                                                placeholder="**********"
                                                type="password"
                                                autoComplete="off"
                                                aria-autocomplete="none"
                                                disabled={progress || code?.length === 0}
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>
                    <Button type="submit" disabled={progress} className="mt-2">
                        {progress && (
                            <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
                        )}
                        Continue
                    </Button>
                    <RequestConfirmationButton email={email} />
                </div>
            </form>
        </Form>
        </>
    )
}