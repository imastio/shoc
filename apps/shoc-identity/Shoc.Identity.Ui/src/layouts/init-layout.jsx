import Loader from "@/components/generic/loader";
import useAuth from "@/providers/auth-provider/use-auth";
import { Outlet } from "react-router-dom";

export default function InitLayout(){
    const auth = useAuth();

    if(auth.progress){
        return <Loader />
    }

    return <Outlet />
}