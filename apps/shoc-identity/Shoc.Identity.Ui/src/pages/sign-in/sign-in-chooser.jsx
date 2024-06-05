import Icons from "@/components/generic/icons"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useState } from "react"
import { useSearchParams } from "react-router-dom"
import useNavigateSearch from "@/hooks/use-navigate-search"
import useAuthorizeContext from "@/hooks/use-authorize-context"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import SignInForm from "./sign-in-form"
import useSignInMethod from "./use-sign-in-method"

export default function SignInChooser({ className, ...props }) {
    const [progress, setProgress] = useState(false);
    const authorizeContext = useAuthorizeContext();
    const method = useSignInMethod();
    const navigateSearch = useNavigateSearch();
    
    return (
        <div className={cn("grid gap-6", className)} {...props}>
            {method === 'magic-link' && <div>Passwordless</div>}
            {method === 'password' && <SignInForm />}

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
                    disabled={progress}
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
                    disabled={progress}
                    onClick={() => navigateSearch({ method: 'password' })}>
                    <Icons.password className="mr-2 h-4 w-4" />
                    {" "}
                    Password
                </Button>
            )}
        </div>
    )
}