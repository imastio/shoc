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
import ConfirmForm from "./recover-password-form";
import useSignIn from "@/hooks/auth/use-sign-in";
import AuthenticatedRedirect from "@/components/auth/authenticated-redirect";

export default function RecoverPasswordPage({ }) {
    
    const [searchParams] = useSearchParams();
    const authorizeContext = useAuthorizeContext();
    const navigateSearch = useNavigateSearch();
    const navigate = useNavigate();
    const navigateExt = useNavigateExt();
    const session = useSession();
    
    if (session.authenticated) {
      return <AuthenticatedRedirect />;
    }

    return <>
    <Helmet title="Recover your password" />
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
                Recover password
              </h1>
            </div>
            <ConfirmForm />
          </div>
        </div>
      </div>
    </>

}