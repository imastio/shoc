import { ServerActionContext, ServerActionFunction, ServerActionInput } from "./types";

export function defineServerAction<T extends ServerActionFunction>(func: T): T {
  return async function (input: ServerActionInput, context: ServerActionContext): Promise<any> {

    return await func(input, context);
    
  } as T;
}