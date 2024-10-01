import { auth } from "@/addons/auth";
import { NextContext, NextMiddlewareDelegate, shouldContinueMiddleware } from ".";
import PUBLIC_PATHS from "@/app/_components/public-paths";
import { NextResponse } from "next/server";

export default async function sessionMiddleware(context: NextContext, next: NextMiddlewareDelegate): Promise<void> {

    // skip prefetch requests
    if (context.request.headers.has('next-router-prefetch') || context.request.headers.get('purpose') === 'prefetch') {
        await next(context);
        return;
    }

    // get pathname
    const pathname = context.request.nextUrl.pathname;

    // check if public
    const isPublic = PUBLIC_PATHS.some(pattern => pattern.test(pathname))

    // do session call
    const session = await auth();

    // if URL is public or is api url
    if(isPublic || pathname.startsWith('/api') || pathname.startsWith('/well-known') || pathname.startsWith('/schemas')){

        if (shouldContinueMiddleware(context.response)) {
            await next(context)
        } 
        return;
    }

    const search = new URLSearchParams();
    search.set('next', pathname || '/')

    if(session?.error === 'refresh_error'){
        search.set('reason', 'expired');
    }

    if(!session || session.error){
        console.log("No session detected")
        context.response = NextResponse.redirect(new URL(`/sign-in?${search.toString()}`, context.request.url))
    }

    // continue down the chain if need to 
    if (shouldContinueMiddleware(context.response)) {
        await next(context)
    }
}