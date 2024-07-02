import { AxiosError } from "axios";
import { ApiError, ServerActionError } from "./error-types";
import ErrorDefinitions from "./error-definitions";

const mapError = (apiError: ApiError): ServerActionError => ({
    kind: apiError.kind,
    code: apiError.code,
    message: apiError.message,
    payload: apiError.payload
});

export default function toServerActionErrors(error: Error | AxiosError | unknown): ServerActionError[] {
    if (error instanceof ApiError) {
        return [mapError(error)];
    }

    if (error instanceof AxiosError) {

        const isEnlightError = error.response &&
            error.response.headers &&
            error.response.headers['x-shoc-error'] === 'aggregate';

        if (isEnlightError && error.response?.data?.errors) {
            return error.response.data.errors.map(mapError)
        }

        switch (error.response?.status) {
            case 400:
                return [mapError(ErrorDefinitions.validation())];
            case 401:
                return [mapError(ErrorDefinitions.notAuthenticated())];
            case 403:
                return [mapError(ErrorDefinitions.accessDenied())];
            case 404:
                return [mapError(ErrorDefinitions.notFound())];
            default:
                return [mapError(ErrorDefinitions.unknown())];
        }
    }

    if(error instanceof Error && error.message === 'NEXT_REDIRECT'){
        return [mapError(ErrorDefinitions.notAllowed())];
    }

    return [mapError(ErrorDefinitions.unknown())];
}

export function assertErrors(errors?: any[] | null){
    if(!errors || errors.length === 0){
        return;
    }

    throw new Error(JSON.stringify({ errors }));
}