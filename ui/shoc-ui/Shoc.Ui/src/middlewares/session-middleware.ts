import { auth } from "@/addons/auth";
import { NextContext, NextMiddlewareDelegate, shouldContinueMiddleware } from ".";

export default async function sessionMiddleware(context: NextContext, next: NextMiddlewareDelegate): Promise<void> {

    // skip prefetch requests
    if (context.request.headers.has('next-router-prefetch') || context.request.headers.get('purpose') === 'prefetch') {
        await next(context);
        return;
    }

    await auth();

    // continue down the chain if need to 
    if (shouldContinueMiddleware(context.response)) {
        await next(context)
    }
}