import Loader from "@/components/generic/loader";
import useOidc from "@/providers/oidc-provider/use-oidc";
import useSession from "@/providers/session-provider/use-session";
import { Outlet } from "react-router-dom";

export default function InitLayout(){
    const session = useSession();
    const oidc = useOidc();

    if(session.progress || oidc.progress){
        return <Loader />
    }

    return <Outlet />
}