import Link from "next/link";
import AppHeaderRightUser from "./app-header-right-user";
import { ReactNode } from "react";

export default function AppHeader({ mobileSidebar }: { mobileSidebar?: ReactNode }) {
    return <header className="flex h-14 items-center gap-4 border-b bg-muted/40 px-4 lg:h-[60px] lg:px-6">
        {mobileSidebar}
        <div className="flex h-14 items-center border-b pr-4 lg:h-[60px] lg:pr-6">
            <Link prefetch={false} href="/workspaces" className="flex items-center gap-2 font-semibold" >
                <span className="">Shoc Platform</span>
            </Link>
        </div>
        <div className="w-full flex-1">
        </div>
        <AppHeaderRightUser />
    </header>
}