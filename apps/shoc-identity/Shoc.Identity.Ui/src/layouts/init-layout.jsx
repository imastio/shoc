import Loader from "@/components/generic/loader";
import useSession from "@/providers/session-provider/use-session";
import { Outlet } from "react-router-dom";

export default function InitLayout(){
    const session = useSession();

    if(session.progress){
        return <Loader />
    }

    return <Outlet />
}