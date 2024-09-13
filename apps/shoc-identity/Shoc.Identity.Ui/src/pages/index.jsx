import AuthLeftCard from "@/components/auth/auth-left-card";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";
import useSession from "@/providers/session-provider/use-session";
import { useEffect } from "react";
import { Helmet } from "react-helmet-async";
import { useIntl } from "react-intl";
import { useNavigate } from "react-router-dom";

export default function IndexPage({ }) {

    const navigate = useNavigate();
    const session = useSession();
    const intl = useIntl();

    useEffect(() => {
        if(!session.authenticated){
            navigate('/sign-in')
        }
    }, [navigate, session.authenticated]);

    if(!session.authenticated){
        return false;
    }

    return <>
    <Helmet title={session.user?.fullName} />
      <div className="container relative grid h-dvh flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
        <AuthLeftCard />

        <div className={cn("lg:p-8", session.authenticated ? "" : "hidden")}>
          <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
            <div className="flex flex-col space-y-2 text-left">
              <h1 className="text-2xl font-semibold tracking-tight">
                {intl.formatMessage({id: 'auth.signedIn.greeting'}, { name: session.user?.fullName || 'Anonymous' })}
              </h1>
            </div>
            <p className="text-left text-sm text-muted-foreground">
              {intl.formatMessage({id: 'auth.signedIn.successNotice'}, { email: <b>{session.user?.email}</b> })}
            </p>
            <p className="text-left text-sm text-muted-foreground">
              {intl.formatMessage({id: 'auth.signedIn.continueNotice'})}
            </p>
            
            <Button variant="default" onClick={() => navigate('/sign-out')}>
              {intl.formatMessage({id: 'auth.signOut'})}
            </Button>
          </div>
        </div>
      </div>
    </>;
}