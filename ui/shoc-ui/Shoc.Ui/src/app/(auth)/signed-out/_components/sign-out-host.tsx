"use client"
import LoadingContainer from "@/components/general/loading-container";
import { signOut, useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import { useCallback, useEffect } from "react";

export default function SignOutHost(){

    const session = useSession();
    const authenticated = session?.status === 'authenticated' && !session?.data?.error;
    const router = useRouter();

    const processSignOut = useCallback(async () => {
        if(authenticated){
            await signOut()
        }
        router.replace('/')
    }, [authenticated, router])

    useEffect(() => {
        processSignOut()
    }, [processSignOut])

    return <LoadingContainer className="w-full h-screen" loading={session?.status !== 'unauthenticated' }>
        <></>
    </LoadingContainer>
}