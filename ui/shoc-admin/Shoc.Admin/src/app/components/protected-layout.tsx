"use client"

import { rpcDirect } from "@/server-actions/rpc";
import { useSession } from "next-auth/react";
import { useEffect } from "react";

export default function ProtectedLayout({ children }: Readonly<{
    children: React.ReactNode;
}>) {
    const session = useSession();

    useEffect(() => {

        if(session.status === 'unauthenticated'){
            rpcDirect('auth/signIn')
        }

    }, [session]);

    return <>
        {children}
    </>

}