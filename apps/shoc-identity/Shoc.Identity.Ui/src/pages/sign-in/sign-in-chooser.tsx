import Icons from "@/components/generic/icons"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import useNavigateSearch from "@/hooks/use-navigate-search"
import SignInForm from "./sign-in-form"
import useSignInMethod from "./use-sign-in-method"
import SignInMagicLinkForm from "./sign-in-magic-link-form"

export default function SignInChooser({ className = '', ...props }) {
    const method = useSignInMethod();
    const navigateSearch = useNavigateSearch();
    
    return (
        <div className={cn("grid gap-6", className)} {...props}>
            {method === 'magic-link' && <SignInMagicLinkForm />}
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
                    onClick={() => navigateSearch({ method: 'password' })}>
                    <Icons.password className="mr-2 h-4 w-4" />
                    {" "}
                    Password
                </Button>
            )}
        </div>
    )
}