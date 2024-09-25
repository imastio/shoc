import { ResolvedContext } from "@/core/types";
import { getAuthenticatedContext } from "./session-service";
import { ApiError } from "@/error-handling/error-types";
import { resolveError } from "@/error-handling/error-codes";
import { AxiosError } from "axios";

interface ClientContext {
    apiRoot: string,
    token: string
}

export default async function clientGuard(context: ResolvedContext, action: (ctx: ClientContext) => Promise<any>) {


    const auth = await getAuthenticatedContext(context.providerUrl);


    try {
        return (await action({ apiRoot: context.providerUrl.toString(), token: auth.accessToken })).data
    }
    catch (error) {
        if (error instanceof ApiError) {
            throw Error(error.message || resolveError(error.code));
        }

        if (error instanceof AxiosError) {

            const isShocError = error.response &&
                error.response.headers &&
                error.response.headers['x-shoc-error'] === 'aggregate';

            if (isShocError && error.response?.data?.errors) {
                const first = error.response.data.errors[0]
                throw Error(error?.message || resolveError(error?.code || ''))
            }

            throw Error(error.message);
        }
    }
}