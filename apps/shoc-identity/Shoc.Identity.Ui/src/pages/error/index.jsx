import { authClient, clientGuard } from "@/clients";
import AuthLeftCard from "@/components/auth/auth-left-card";
import { Button } from "@/components/ui/button";
import useAuthorizeContext from "@/hooks/use-authorize-context";
import { cn } from "@/lib/utils";
import { useCallback, useEffect, useState } from "react";
import { Helmet } from "react-helmet-async";
import { useNavigate } from "react-router-dom";

export default function ErrorPage({ }) {

    const navigate = useNavigate();
    const authorizeContext = useAuthorizeContext();
    const [progress, setProgress] = useState(authorizeContext.errorId?.length > 0);
    const [data, setData] = useState({});

    const load = useCallback(async () => {

        setProgress(true);

        const result = await clientGuard(() => authClient.getErrorDetails({
            errorId: authorizeContext.errorId
        }));

        setProgress(false);
        setData(result.error ? {} : result.payload)

    }, [authorizeContext.errorId])
    
    useEffect(() => {
        if(authorizeContext.errorId?.length > 0){
            load();
        }
    }, [load])

    return <>
    <Helmet title="Error" />
      <div className="container relative grid h-dvh flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
        <AuthLeftCard />
        <div className={cn("lg:p-8")}>
          <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
            <div className="flex flex-col space-y-2 text-left">
              <h1 className="text-2xl font-semibold tracking-tight">
                Error
              </h1>
            </div>
            <p className="text-left text-sm text-muted-foreground">
                Unfortunately, we could not process your request due to an error.
            </p>
            {data.error && <p className="text-left text-sm text-muted-foreground">
                Error: {data.error}
            </p>}
            <Button variant="default" onClick={() => navigate('/')} disabled={progress}>
              Homepage
            </Button>
          </div>
        </div>
      </div>
    </>;
}