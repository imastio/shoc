"use client"

import Loader from "@/components/general/loader";
import { usePathname } from "next/navigation";
import PublicInitLayout from "./public-init-layout";
import PrivateInitLayout from "./private-init-layout";
import AccessGuardLayout from "./access-guard-layout";

const PUBLIC_ROUTES = [
    '/signed-out'
];

export default function InitLayout({ children }: { children: React.ReactNode }) {
    const pathname = usePathname();

    if (!pathname) {
        return <Loader />
    }

    if (PUBLIC_ROUTES.includes(pathname)) {
        return <PublicInitLayout>
            {children}
        </PublicInitLayout>
    }
    return <PrivateInitLayout>
        <AccessGuardLayout>
            {children}
        </AccessGuardLayout>
    </PrivateInitLayout>
}