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
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"
import { RocketIcon } from "@radix-ui/react-icons"

export default function SignInMagicLinkForm() {
    const [searchParams, setSearchParams] = useSearchParams();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);
    const [done, setDone] = useState(false);
    const intl = useIntl();
    const authorizeContext = useAuthorizeContext();
    const method = useSignInMethod();
    const navigateExt = useNavigateExt();

    const formSchema = z.object({
        email: z.string().email('Enter a valid email!'),
    })

    const form = useForm({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: authorizeContext.loginHint,
        },
        shouldUseNativeValidation: false
    })

    const requestLink = useCallback(async ({ email, returnUrl }) => {

        setProgress(true);
        setErrors([]);
        setDone(false);

        const result = await clientGuard(() => authClient.requestOtp({
            email,
            returnUrl,
            signinMethod: "magic_link",
            deliveryMethod: "email"
        }));

        setProgress(false);

        if (result.error) {
            if (result.payload?.errors?.some(e => e.code === "IDENTITY_UNVERIFIED_EMAIL")) {
                navigateExt({
                    pathname: "/confirm",
                    search: `?${searchParams.toString()}`
                })
            }

            setErrors(result.payload?.errors || []);
            return;
        }

        setDone(true);

    }, [navigateExt]);

    async function onSubmit(values) {
        await requestLink({ email: values.email, returnUrl: authorizeContext.returnUrl });
    }

    return (
        <>
            <ErrorAlert errors={errors} title={intl.formatMessage({ id: 'auth.signIn.requestLink.unable' })} />
            <Alert className={cn(done ? "" : "hidden")}>
                <RocketIcon className="h-4 w-4" />
                <AlertTitle>Great!</AlertTitle>
                <AlertDescription>
                    You should receive an email with a magic link!
                </AlertDescription>
            </Alert>
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
                                                disabled={progress || done}
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                        <FormDescription>
                                            We will send you an email with a special link to sign in with.
                                        </FormDescription>
                                    </FormItem>
                                )}
                            />
                        </div>

                        <Button type="submit" disabled={progress || done}>
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