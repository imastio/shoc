import Loader from "@/components/generic/loader";
import { Toaster } from "@/components/ui/toaster";
import LocaleProvider from "@/i18n/local-provider";
import { Outlet } from "react-router-dom";

export default function GlobalLayout() {

    return <LocaleProvider>
        <Toaster />
        <Outlet />
    </LocaleProvider>
}