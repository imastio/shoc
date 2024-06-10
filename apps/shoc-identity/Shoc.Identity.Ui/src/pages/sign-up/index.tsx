import useNavigateExt from "@/hooks/use-navigate-ext";
import { useEffect } from "react";
import { useSearchParams } from "react-router-dom";

export default function SingUpPage() {
    const navigateExt = useNavigateExt();
    const [searchParams] = useSearchParams();

    useEffect(() => {
        navigateExt({
            pathname: "/sign-in",
            search: `?${searchParams.toString()}`,
            searchOverrides: { prompt: 'create' }
        }, { replace: true });

    }, [navigateExt])

    return false;
}