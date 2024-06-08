import { Button, buttonVariants } from "@/components/ui/button"
import { Link, useNavigate } from "react-router-dom"
import { cn } from "@/lib/utils"
import AuthLeftCard from "@/components/auth/auth-left-card"
import PrivacyNotice from "@/components/auth/privacy-notice"
import SignInChooser from "./sign-in-chooser"
import useAuthorizeContext from "@/hooks/use-authorize-context"
import useNavigateSearch from "@/hooks/use-navigate-search"
import SignUpForm from "./sign-up-form"
import useSession from "@/providers/session-provider/use-session"
import { useEffect } from "react"
import { Helmet } from "react-helmet-async"
import useNavigateExt from "@/hooks/auth/use-navigate-ext"
import SignOutForm from "./sign-out-form"

export default function SignOutPage() {
  const authorizeContext = useAuthorizeContext();
  const navigateSearch = useNavigateSearch();
  const navigate = useNavigate();
  const navigateExt = useNavigateExt();
  const session = useSession();

  useEffect(() => {
    if(!session.authenticated){
      navigate('/sign-in', { replace: true })
    }
  }, [session.authenticated, navigate])

  if (!session.authenticated) {
    return false;
  }

  return (
    <>
      <Helmet title="Sign out" />
      <div className="container relative grid h-dvh flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
        <AuthLeftCard />

        <div className={cn("lg:p-8")}>
          <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
            <div className="flex flex-col space-y-2 text-left">
              <h1 className="text-2xl font-semibold tracking-tight">
                Sign out
              </h1>
            </div>
            <p className="text-left text-sm text-muted-foreground">
              You are about to sign out from the Shoc Platform.
            </p>
            <p className="text-left text-sm text-muted-foreground">
              Are you sure you want to terminate your current session?
            </p>
            <SignOutForm />
          </div>
        </div>
      </div>
    </>
  )
}