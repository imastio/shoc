import AuthLeftCard from "@/components/auth/auth-left-card";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";
import { Helmet } from "react-helmet-async";
import { useIntl } from "react-intl";
import { useNavigate } from "react-router-dom";

export default function NotFoundPage({ }) {

    const navigate = useNavigate();
    const intl = useIntl();
    
    return <>
    <Helmet title={intl.formatMessage({id: 'auth.pages.notFound'})} />
      <div className="container relative grid h-dvh flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
        <AuthLeftCard />
        <div className={cn("lg:p-8")}>
          <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
            <div className="flex flex-col space-y-2 text-left">
              <h1 className="text-2xl font-semibold tracking-tight">
                {intl.formatMessage({id: 'auth.pages.notFound'})}
              </h1>
            </div>
            <p className="text-left text-sm text-muted-foreground">
              {intl.formatMessage({id: 'auth.pages.notFound.notice'})}
            </p>
            <Button variant="default" onClick={() => navigate('/')}>
              {intl.formatMessage({id: 'auth.homepage'})}
            </Button>
          </div>
        </div>
      </div>
    </>;
}