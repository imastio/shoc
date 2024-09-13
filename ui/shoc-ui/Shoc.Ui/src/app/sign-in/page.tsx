import { auth } from "@/addons/auth";
import { redirect, RedirectType } from "next/navigation";
import SignInWrapper from "./_components/sign-in-wrapper";

export default async function SignInPage({ searchParams }: { searchParams: {[key: string]: string | string[] | undefined} }) {
    
    const session = await auth();

    let next = searchParams['next'];

    if(Array.isArray(next)){
        next = next[0];
    }

    if(!next?.startsWith('/')){
        next = '/'
    }

    if(session && !session.error){
        redirect(next, RedirectType.replace);
    }

    let reason = searchParams['reason'];
    
    if(Array.isArray(reason)){
        reason = reason[0]
    }
    
    if(!reason){
        reason = ''
    }
    
    return (
        <div className="min-h-screen flex">
            <SignInWrapper next={next} reason={reason} />
        </div>
    )
}