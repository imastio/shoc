import AuthLeftCard from "@/components/auth/auth-left-card";
import { Button } from "@/components/ui/button";
import useNavigateExt from "@/hooks/auth/use-navigate-ext";
import useAuthorizeContext from "@/hooks/use-authorize-context";
import useNavigateSearch from "@/hooks/use-navigate-search";
import { cn } from "@/lib/utils";
import useSession from "@/providers/session-provider/use-session";
import { useEffect } from "react";
import { Helmet } from "react-helmet-async";
import { useNavigate, useSearchParams } from "react-router-dom";
import ConfirmForm from "./confirm-form";

export default function ConfirmPage({ }) {

    const [searchParams] = useSearchParams();
    const authorizeContext = useAuthorizeContext();
    const navigateSearch = useNavigateSearch();
    const navigate = useNavigate();
    const navigateExt = useNavigateExt();
    const session = useSession();
    
    return <>
    <Helmet title="Confirm your email address" />
      <div className="container relative grid h-dvh flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
        <AuthLeftCard />

        <Button variant="ghost"
          onClick={() => 
            navigateExt({
                pathname: "/sign-in",
                search: `?${searchParams.toString()}`
            })
          }
          className={cn("absolute right-4 top-4 md:right-8 md:top-8")}
        >
          Sign in
        </Button>

        <div className={cn("lg:p-8", session.authenticated ? "hidden" : "")}>
          <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
            <div className="flex flex-col space-y-2 text-left">
              <h1 className="text-2xl font-semibold tracking-tight">
                Confirm your email
              </h1>
            </div>
            <ConfirmForm />
          </div>
        </div>
        <div className={cn("lg:p-8", session.authenticated ? "" : "hidden")}>
            <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
              <div className="flex flex-col space-y-2 text-left">
                <h1 className="text-2xl font-semibold tracking-tight">
                  Hey, {session.user?.fullName || 'Anonymous'}!
                </h1>
              </div>
              <p className="text-left text-sm text-muted-foreground">
                Your email <b>{session.user?.email}</b> is already verified, so you do not need to worry about that.
              </p>
              <p className="text-left text-sm text-muted-foreground">
                You can go to our homepage for more!
              </p>

              <Button variant="default" onClick={() => navigate('/')}>
                Homepage
              </Button>
            </div>
          </div>
      </div>
    </>

}