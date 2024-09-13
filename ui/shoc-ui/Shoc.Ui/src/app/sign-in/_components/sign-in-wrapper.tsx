"use client"

import LoadingContainer from "@/components/general/loading-container";
import { useSession } from "next-auth/react";
import SignInForm from "./sign-in-form";
import { useEffect } from "react";
import { useRouter } from "next/navigation";

export default function SignInWrapper({ next, reason }: { next: string, reason: string }) {

    const session = useSession();
    const router = useRouter();

    const authenticated = session.status === 'authenticated' && !session.data.error;

    useEffect(() => {
        if(authenticated){
            router.replace(next);
        }
    }, [router, authenticated, next])

    return <LoadingContainer className="w-full h-min-screen m-auto" loading={session.status === 'loading'}>
        {!authenticated && <SignInForm expired={reason === 'expired'} next={next} />}
    </LoadingContainer>
}