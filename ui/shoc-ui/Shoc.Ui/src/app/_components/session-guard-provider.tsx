"use client"

import { useSession } from "next-auth/react";
import { usePathname } from "next/navigation";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import PUBLIC_PATHS from "./public-paths";

export default function SessionGuardProvider({ children }: { children: React.ReactNode }) {
    const session = useSession();  
    const pathname = usePathname() || '/';
    const router = useRouter()
    const user = session.data?.user;

    const isPublic = PUBLIC_PATHS.some(pattern => pattern.test(pathname));

    useEffect(() => {

        if(isPublic){
            return;
        }

        if (session.status === 'unauthenticated' || session.data?.error) {

            const search = new URLSearchParams();
            search.set('next', pathname || '/')

            if(session.data?.error === 'refresh_error'){
                search.set('reason', 'expired');
            }

            router.push(`/sign-in?${search.toString()}`)
        }

    }, [session, isPublic, pathname, router]);

    return <>
        {children}
    </>
}