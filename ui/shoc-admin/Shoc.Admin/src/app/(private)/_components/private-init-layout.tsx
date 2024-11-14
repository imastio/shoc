"use client"

import Loader from "@/components/general/loader";
import { rpcDirect } from "@/server-actions/rpc";
import { useSession } from "next-auth/react";
import { useEffect } from "react";

export default function PrivateInitLayout({ children }: { children: React.ReactNode }) {
    const session = useSession();

    const progress = session.status === 'loading';

    const user = session.data?.user;

    useEffect(() => {
        if (session.status === 'unauthenticated' || session.data?.error) {
            rpcDirect('auth/signIn')
        }

    }, [session, user]);

    if (progress || session.status === 'unauthenticated' || !user) {
        return <Loader />
    }

    return children
}