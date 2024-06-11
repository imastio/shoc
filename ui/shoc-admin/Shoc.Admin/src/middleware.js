import { NextResponse } from 'next/server';
import { chainMiddleware } from './middlewares';
import cspMiddleware from './middlewares/csp-middleware';
import apiForwardMiddleware from './middlewares/api-forward-middleware';

const terminalMiddleware = async (context, next) => {
  await next(context)
}

const middlewaresChain = chainMiddleware([apiForwardMiddleware, cspMiddleware, terminalMiddleware])

export async function middleware(request, event) {

  const context = {
    request: request,
    event: event,
    response: NextResponse.next()
  }

  await middlewaresChain(context);

  return context.response;
}

// only applies this middleware to files in the app directory
export const config = {
  matcher: '/((?!static|.*\\..*|_next).*)'
};
