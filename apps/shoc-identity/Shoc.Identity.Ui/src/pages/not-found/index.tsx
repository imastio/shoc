import AuthLeftCard from "@/components/auth/auth-left-card";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";
import { Helmet } from "react-helmet-async";
import { useNavigate } from "react-router-dom";

export default function NotFoundPage({ }) {

    const navigate = useNavigate();
    
    return <>
    <Helmet title="Page not found" />
      <div className="container relative grid h-dvh flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
        <AuthLeftCard />
        <div className={cn("lg:p-8")}>
          <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
            <div className="flex flex-col space-y-2 text-left">
              <h1 className="text-2xl font-semibold tracking-tight">
                Page not found
              </h1>
            </div>
            <p className="text-left text-sm text-muted-foreground">
              The content you are looking for could not be found.
            </p>
            <Button variant="default" onClick={() => navigate('/')}>
              Homepage
            </Button>
          </div>
        </div>
      </div>
    </>;
}