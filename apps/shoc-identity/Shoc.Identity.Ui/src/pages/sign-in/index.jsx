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

export default function SignInPage() {
  const authorizeContext = useAuthorizeContext();
  const navigateSearch = useNavigateSearch();
  const navigate = useNavigate();
  const session = useSession();

  let title = null;
  if (authorizeContext.prompt === 'create') {
    title = 'Create an account'
  }

  if (authorizeContext.prompt === 'login') {
    title = 'Sign in'
  }

  return (
    <>
      <Helmet title={title} />
        <div className="container relative grid h-dvh flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
          <AuthLeftCard />

          {authorizeContext.prompt === 'login' && <Button variant="ghost"
            onClick={() => {
              navigateSearch({ prompt: 'create' })
            }}
            className={cn(
              session.authenticated ? "hidden" : "",
              "absolute right-4 top-4 md:right-8 md:top-8"
            )}
          >
            Create account
          </Button>
          }

          {authorizeContext.prompt === 'create' && <Button variant="ghost"
            onClick={() => {
              navigateSearch({ prompt: 'login' })
            }}
            className={cn(
              session.authenticated ? "hidden" : "",
              "absolute right-4 top-4 md:right-8 md:top-8"
            )}
          >
            Sign in
          </Button>
          }

          <div className={cn("lg:p-8", session.authenticated ? "hidden" : "")}>
            <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
              <div className="flex flex-col space-y-2 text-left">
                <h1 className="text-2xl font-semibold tracking-tight">
                  {authorizeContext.prompt === 'login' && 'Sign in to continue'}
                  {authorizeContext.prompt === 'create' && 'Create an account'}
                </h1>
              </div>
              {authorizeContext.prompt === 'login' && <SignInChooser />}
              {authorizeContext.prompt === 'create' && <SignUpForm />}
              <PrivacyNotice />
            </div>
          </div>
          <div className={cn("lg:p-8", session.authenticated ? "" : "hidden")}>
            <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
              <div className="flex flex-col space-y-2 text-left">
                <h1 className="text-2xl font-semibold tracking-tight">
                  Welcome, {session.user?.fullName || 'Anonymous'}!
                </h1>
              </div>
              <p className="text-left text-sm text-muted-foreground">
                You are already signed in using your account on <b>{session.user?.email}</b> email.
              </p>
              <p className="text-left text-sm text-muted-foreground">
                If you would like to sign in to another account, please sign out first!
              </p>

              <Button variant="default" onClick={() => navigate('/sign-out')}>
                Sign out
              </Button>
            </div>
          </div>
        </div>
      </>
      )
}