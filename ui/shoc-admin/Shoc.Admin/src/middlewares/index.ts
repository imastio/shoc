import { NextFetchEvent, NextRequest, NextResponse } from "next/server"

export type NextContext = {
  request: NextRequest,
  event: NextFetchEvent,
  response: NextResponse
};

export type NextMiddlewareDelegate = (
  context: NextContext, 
  next?: NextMiddlewareDelegate 
) => Promise<void>;

export type NextMiddlewareChain = (middlewares: NextMiddlewareDelegate[]) => (
  context: NextContext
) => Promise<void>;

export function shouldContinueMiddleware(response: NextResponse): boolean {
  return response.headers.get('x-middleware-next') === '1';
}

export const chainMiddleware: NextMiddlewareChain = (middlewares: NextMiddlewareDelegate[]) => {  
  return async (context: NextContext) => {

    // Define a function to execute the middleware chain
    const executeMiddleware = async (index: number): Promise<void> => {
      if (index < middlewares.length) {
        const currentMiddleware = middlewares[index];

        // Define a next function to pass to the current middleware
        const nextMiddleware = async (nextContext: NextContext) => {
          // Call the next middleware in the chain
          await executeMiddleware(index + 1);
        };

        // Execute the current middleware
        await currentMiddleware(context, nextMiddleware);
      }
    };

    // Start executing the middleware chain from the first middleware
    await executeMiddleware(0);
  };
};
