import { ServerActionError } from "@/addons/error-handling/error-types";

export type ServerActionInput = any;

export type ServerActionContext = { 
  locale?: string | null | undefined, 
  captchaChallenge?: any | null | undefined 
};

export type ServerActionFunction = (input?: ServerActionInput, context?: ServerActionContext) => any;

export type ServerActionResult = { data?: any | undefined | null, errors: ServerActionError[] | undefined | null };
