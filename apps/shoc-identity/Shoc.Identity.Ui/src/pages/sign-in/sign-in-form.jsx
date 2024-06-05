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
import useSignInMethod from "./use-sign-in-method"
import useNavigateExt from "@/hooks/auth/use-navigate-ext"
import { authClient, clientGuard } from "@/clients"
import ErrorAlert from "@/components/generic/error-alert"
import { useIntl } from "react-intl";


export default function SignInForm() {
    const [searchParams, setSearchParams] = useSearchParams();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);
    const [flowResult, setFlowResult] = useState({});
    const intl = useIntl();
    const authorizeContext = useAuthorizeContext();
    const method = useSignInMethod();
    const navigateExt = useNavigateExt();

    const formSchema = z.object({
        email: z.string().email('Enter a valid email!'),
        password: z.string().min(6, 'Password must have at least 6 characters!')
    })

    const form = useForm({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: authorizeContext.loginHint,
            password: ''
        },
        shouldUseNativeValidation: false
    })

    const signIn = useCallback(async ({ email, password, returnUrl }) => {
        
        setProgress(true);
        setErrors([]);

        const result = await clientGuard(() => authClient.signin({ email, password, returnUrl }));

        setProgress(false);
        
        if(result.error){

            if (result.payload?.errors?.some(e => e.code === "IDENTITY_UNVERIFIED_EMAIL")) {
                navigateExt({
                  pathname: "/confirm",
                  search: `?${searchParams.toString()}`
                })
              }

            setErrors(result.payload?.errors || []);
            return;
        }

        setFlowResult(result.payload || {});

    }, [navigateExt]);

    async function onSubmit(values) {
        await signIn({ email: values.email, password: values.password, returnUrl: authorizeContext.returnUrl });
    }

    return (
        <>
        <ErrorAlert errors={errors} title={intl.formatMessage({id: 'auth.signIn.unable'})} />
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
                            name="password"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Password</FormLabel>
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
                        Continue
                    </Button>
                </div>
            </form>
        </Form>
        </>
    )
}