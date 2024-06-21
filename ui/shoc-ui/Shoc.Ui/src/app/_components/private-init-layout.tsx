"use client"

import Loader from "@/components/general/loader";
import { rpcDirect } from "@/server-actions/rpc";
import { useSession } from "next-auth/react";
import { useEffect } from "react";

export default function PrivateInitLayout({ children }: { children: React.ReactNode }) {
    const session = useSession();

    const authProgress = session.status === 'loading';

    const progress = authProgress;

    useEffect(() => {

        if(session.status === 'unauthenticated'){
            rpcDirect('auth/signIn')
        }

    }, [session]);

    if(progress){
        return <Loader />
    }

    return <>
        {children}
        </>
}