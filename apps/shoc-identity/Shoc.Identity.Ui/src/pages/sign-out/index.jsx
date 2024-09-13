import { useNavigate } from "react-router-dom"
import { cn } from "@/lib/utils"
import AuthLeftCard from "@/components/auth/auth-left-card"
import useSession from "@/providers/session-provider/use-session"
import { useEffect } from "react"
import { Helmet } from "react-helmet-async"
import SignOutForm from "./sign-out-form"
import { useIntl } from "react-intl"

export default function SignOutPage() {
  const navigate = useNavigate();
  const session = useSession();
  const intl = useIntl();

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
      <Helmet title={intl.formatMessage({id: 'auth.signOut'})} />
      <div className="container relative grid h-dvh flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
        <AuthLeftCard />

        <div className={cn("lg:p-8")}>
          <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
            <div className="flex flex-col space-y-2 text-left">
              <h1 className="text-2xl font-semibold tracking-tight">
                {intl.formatMessage({id: 'auth.signOut'})}
              </h1>
            </div>
            <p className="text-left text-sm text-muted-foreground">
              {intl.formatMessage({id: 'auth.signOut.notice'})}
            </p>
            <p className="text-left text-sm text-muted-foreground">
            {intl.formatMessage({id: 'auth.signOut.confirmationNotice'})}
            </p>
            <SignOutForm />
          </div>
        </div>
      </div>
    </>
  )
}