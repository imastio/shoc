import { auth, getJwt } from "@/addons/auth";
import { NextContext, NextMiddlewareDelegate, shouldContinueMiddleware } from ".";
import { NextResponse } from "next/server";
import { NextApiRequest } from "next";

export default async function apiForwardMiddleware(context: NextContext, next: NextMiddlewareDelegate): Promise<void> {

    console.log("Enter api forward middleware", context.request.nextUrl.pathname);

    // skip if not a /handle route
    if (!context.request.nextUrl.pathname.startsWith('/handle')) {
        await next(context);
        return;
    }

    // skip prefetch requests
    if (context.request.headers.has('next-router-prefetch') || context.request.headers.get('purpose') === 'prefetch') {
        await next(context);
        return;
    }

    const url = new URL(context.request.url);
    const pathname = url.pathname;
    const search = url.search;

    const transformedPathname = pathname.replace('/handle', '');
    
    let apiRoot = process.env.SHOC_ADMIN_API_ROOT || '';
    if(apiRoot.endsWith('/')){
        apiRoot.replace(/\/$/, "");
    }

    const newUrl = `${apiRoot}${transformedPathname}${search}`;
    
    const session = await auth();
    const jwt = await getJwt(context.request.headers);

    console.log("Forwarding", { url: newUrl, token: jwt?.access_token })

    context.response = NextResponse.rewrite(newUrl, { headers: {
        'Authorization': `Bearer ${jwt?.access_token}`
    } })

    return;
}