import { NextContext, NextMiddlewareDelegate, shouldContinueMiddleware } from ".";

// allowed scripts
const scriptSrc = [
    "*.googletagmanager.com",
    "*.google-analytics.com",
    "*.google.com",
    "*.gstatic.com",
    "*.googleadservices.com",
    "*.doubleclick.net"
].join(' ');

// allowed connect
const connectSrc = [
    "*.google-analytics.com",
    "*.googletagmanager.com",
    "*.google.com",
    "*.doubleclick.net"
].join(' ')

// allowed frames
const frameSrc = [
    "*.google.com"
].join(' ')

export default async function cspMiddleware(context: NextContext, next: NextMiddlewareDelegate): Promise<void> {

    // skip /api and /handle routes
    if (context.request.nextUrl.pathname.startsWith('/api')) {
        await next(context);
        return;
    }

    // skip prefetch requests
    if (context.request.headers.has('next-router-prefetch') || context.request.headers.get('purpose') === 'prefetch') {
        await next(context);
        return;
    }

    // build csp header
    const cspHeader = `
    default-src 'self';
    script-src 'self' 'unsafe-eval' 'unsafe-inline' ${scriptSrc};
    style-src 'self' 'unsafe-inline';
    img-src 'self' blob: data:;
    font-src 'self';
    object-src 'none';
    base-uri 'self';
    form-action 'self';
    frame-ancestors 'none';
    frame-src 'self' ${frameSrc};
    connect-src 'self' ${connectSrc};
    upgrade-insecure-requests;
`
    // replace newline characters and spaces
    const contentSecurityPolicyHeaderValue = cspHeader
        .replace(/\s{2,}/g, ' ')
        .trim();
    
    // set CSP policy and move next
    context.response.headers.set('Content-Security-Policy', contentSecurityPolicyHeaderValue);

    // continue down the chain if need to 
    if (shouldContinueMiddleware(context.response)) {
        await next(context)
    }
}