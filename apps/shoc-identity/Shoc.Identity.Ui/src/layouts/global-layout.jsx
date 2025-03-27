import { Toaster } from "@/components/ui/toaster";
import LocaleProvider from "@/i18n/local-provider";
import OidcProvider from "@/providers/oidc-provider/oidc-provider";
import { Outlet } from "react-router-dom";

export default function GlobalLayout() {

    return <OidcProvider>
        <LocaleProvider>
            <Toaster />
            <Outlet />
        </LocaleProvider>
    </OidcProvider>
}