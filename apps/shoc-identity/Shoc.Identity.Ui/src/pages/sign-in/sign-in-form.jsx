import Icons from "@/components/generic/icons"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useState } from "react"
import { useSearchParams } from "react-router-dom"
import useMethod from "./use-method"
import useNavigateSearch from "@/hooks/use-navigate-search"
import useAuthorizeContext from "@/hooks/use-authorize-context"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"

export default function SignInForm({ className, ...props }) {
    const [searchParams, setSearchParams] = useSearchParams();
    const [isLoading, setIsLoading] = useState(false);
    const authorizeContext = useAuthorizeContext();
    const navigateSearch = useNavigateSearch();

    const method = useMethod();

    const emailRule = { email: z.string().email('Enter a valid email!') }
    const passwordRule = method === 'magic-link' ? {} : {
        password: z.string().min(6, 'Password must have at least 6 characters!')
    }

    const formSchema = z.object({
        ...emailRule,
        ...passwordRule
    })

    const form = useForm({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: authorizeContext.loginHint,
            password: ''
        },
        shouldUseNativeValidation: false
    })

    async function onSubmit(values) {
       console.log("here are the values")
    }

    return (
        <div className={cn("grid gap-6", className)} {...props}>

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
                                                disabled={isLoading}
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>
                        {method === 'password' && <div className="grid gap-1">
                        <FormField
                                control={form.control}
                                name="password"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel >Password</FormLabel>
                                        <FormControl>
                                            <Input
                                                placeholder="**********"
                                                type="password"
                                                autoComplete="off"
                                                aria-autocomplete="none"
                                                disabled={isLoading}
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>}
                        <Button type="submit" disabled={isLoading}>
                            {isLoading && (
                                <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
                            )}
                            Continue
                        </Button>
                    </div>
                </form>
            </Form>
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