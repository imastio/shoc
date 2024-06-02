import Icons from "@/components/generic/icons"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useState } from "react"
import { useSearchParams } from "react-router-dom"
import useMethod from "./use-method"
import useNavigateSearch from "@/hooks/use-navigate-search"
import * as Yup from 'yup';
import { Formik, Form, ErrorMessage } from 'formik';
import useAuthorizeContext from "@/hooks/use-authorize-context"

export default function SignInForm({ className, ...props }) {
    const [searchParams, setSearchParams] = useSearchParams();
    const [isLoading, setIsLoading] = useState(false);
    const authorizeContext = useAuthorizeContext();
    const navigateSearch = useNavigateSearch();

    const method = useMethod();

    const signInSchema = Yup.object().shape({
        email: Yup.string().email('Invalid email').required('Email is required'),
        password: method === 'password' ? Yup.string()
            .min(6, 'Password must be at least 6 characters')
            .required('Password is required') : Yup.string()
    });

    async function onSubmit(event) {
        event.preventDefault()
        setIsLoading(true)

        setTimeout(() => {
            setIsLoading(false)
        }, 3000)
    }

    return (
        <div className={cn("grid gap-6", className)} {...props}>
            <Formik
                initialValues={{
                    email: authorizeContext.loginHint || '',
                    password: ''
                }}
                validationSchema={signInSchema}
                onSubmit={async (values) => {
                    setIsLoading(true);
                    await new Promise((r) => setTimeout(r, 500));
                    setIsLoading(false);
                    alert(JSON.stringify(values, null, 2));
                }}
            >
                <Form>
                    <div className="grid gap-2">
                        <div className="grid gap-1">
                            <Label className="sr-only" htmlFor="email">
                                Email
                            </Label>
                            <Input
                                autoFocus
                                id="email"
                                name="email"
                                placeholder="name@example.com"
                                type="email"
                                autoCapitalize="none"
                                autoComplete="email"
                                autoCorrect="off"
                                disabled={isLoading}
                            />
                            <ErrorMessage name="email" component="div" className="text-red-500" />
                        </div>
                        {method === 'password' && <div className="grid gap-1">
                            <Label className="sr-only" htmlFor="password">
                                Password
                            </Label>
                            <Input
                                id="password"
                                name="password"
                                placeholder="**********"
                                type="password"
                                autoCapitalize="none"
                                autoComplete="current-password"
                                autoCorrect="off"
                                disabled={isLoading}
                            />
                            <ErrorMessage name="password" component="div" className="text-red-500" />
                        </div>}
                        <Button type="submit" disabled={isLoading}>
                            {isLoading && (
                                <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
                            )}
                            Continue
                        </Button>
                    </div>
                </Form>
            </Formik>
            <div className="relative">
                <div className="absolute inset-0 flex items-center">
                    <span className="w-full border-t" />
                </div>
                <div className="relative flex justify-center text-xs uppercase">
                    <span className="bg-background px-2 text-muted-foreground">
                        Or continue with
                    </span>
                </div>
            </div>
            {method === 'password' && (
                <Button
                    variant="outline"
                    type="button"
                    disabled={isLoading}
                    onClick={() => navigateSearch({ method: 'magic-link' })}>
                    <Icons.magicLink className="mr-2 h-4 w-4" />
                    {" "}
                    Magic link
                </Button>
            )}
            {method === 'magic-link' && (
                <Button
                    variant="outline"
                    type="button"
                    disabled={isLoading}
                    onClick={() => navigateSearch({ method: 'password' })}>
                    <Icons.password className="mr-2 h-4 w-4" />
                    {" "}
                    Password
                </Button>
            )}
        </div>
    )
}