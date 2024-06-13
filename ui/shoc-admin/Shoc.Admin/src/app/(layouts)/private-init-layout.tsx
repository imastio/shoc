"use client"

import Loader from "@/components/general/loader";
import useAccess from "@/providers/access-provider/use-access";
import { rpcDirect } from "@/server-actions/rpc";
import { useSession } from "next-auth/react";
import { useEffect } from "react";
import ConsoleLayout from "./console-layout";

export default function PrivateInitLayout({ children }: { children: React.ReactNode }) {
    const session = useSession();

    const authProgress = session.status === 'loading';
    const { progress: accessProgress } = useAccess();

    const progress = authProgress || accessProgress;

    useEffect(() => {

        if(session.status === 'unauthenticated'){
            rpcDirect('auth/signIn')
        }

    }, [session]);

    if(progress){
        return <Loader />
    }

    return <ConsoleLayout>
        {children}
        </ConsoleLayout>
}