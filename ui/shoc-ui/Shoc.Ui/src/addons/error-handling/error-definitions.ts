import { ApiError } from "./error-types";

export default class ErrorDefinitions {

    static notFound(message?: string, code?: string, payload: any = {}) {
        return new ApiError('not_found', code || 'NOT_FOUND_ERROR', message || 'Not Found', payload);
    }

    static validation(message?: string, code?: string, payload: any = {}) {
        return new ApiError('validation', code || 'VALIDATION_ERROR', message || 'Validation Error', payload);
    }

    static data(message?: string, code?: string, payload: any = {}) {
        return new ApiError('data', code || 'DATA_ERROR', message || 'Data Error', payload);
    }

    static accessDenied(message?: string, code?: string, payload: any = {}) {
        return new ApiError('access_denied', code || 'ACCESS_ERROR', message || 'Access Denied', payload);
    }

    static notAuthenticated(message?: string, code?: string, payload: any = {}) {
        return new ApiError('not_authenticated', code || 'AUTHENTICATION_ERROR', message || 'Not Authenticated', payload);
    }

    static notAllowed(message?: string, code?: string, payload: any = {}) {
        return new ApiError('not_allowed', code || 'NOT_ALLOWED_ERROR', message || 'Not Allowed', payload);
    }

    static unknown(message?: string, code?: string, payload: any = {}) {
        return new ApiError('unknown', code || 'UNKNOWN_ERROR', message || 'Unknown Error', payload);
    }
}