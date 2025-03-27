import Icons from "@/components/generic/icons"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import useNavigateSearch from "@/hooks/use-navigate-search"
import SignInForm from "./sign-in-form"
import useSignInMethod from "./use-sign-in-method"
import SignInMagicLinkForm from "./sign-in-magic-link-form"
import { useIntl } from "react-intl"
import {useSearchParams} from "react-router-dom";

export default function SignInChooser({ className = '', ...props }) {
    const method = useSignInMethod();
    const navigateSearch = useNavigateSearch();
    const intl = useIntl();
    const [searchParams] = useSearchParams();

    const returnUrl = searchParams.get('return_url') || searchParams.get('returnUrl') || '/';

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
                        {intl.formatMessage({id: 'auth.signIn.methods.choose'})}
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
                    {intl.formatMessage({id: 'auth.signIn.methods.magicLink'})}
                </Button>
            )}
            {method === 'magic-link' && (
                <Button
                    variant="outline"
                    type="button"
                    onClick={() => navigateSearch({ method: 'password' })}>
                    <Icons.password className="mr-2 h-4 w-4" />
                    {" "}
                    {intl.formatMessage({id: 'auth.signIn.methods.password'})}
                </Button>
            )}
            <Button
                variant="outline"
                type="button"
                onClick={() => window.location.href = `/api-auth/external/google?returnUrl=${returnUrl}`}>
                <Icons.google className="mr-2 h-4 w-4" />
                {" "}
                {intl.formatMessage({id: 'auth.signIn.methods.google'})}
            </Button>
        </div>
    )
}