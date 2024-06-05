import Loader from "@/components/generic/loader";
import LocaleProvider from "@/i18n/local-provider";
import useAuth from "@/providers/auth-provider/use-auth";
import { Outlet } from "react-router-dom";

export default function GlobalLayout() {

    return <LocaleProvider>
        <Outlet />
    </LocaleProvider>
}