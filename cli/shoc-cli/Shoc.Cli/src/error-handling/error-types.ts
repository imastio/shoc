export type ErrorKind = 'not_found' | 'data' | 'validation' | 'access_denied' | 'not_authenticated' | 'not_allowed' | 'unknown';

export class ApiError extends Error {
    kind: ErrorKind;
    code: string;
    payload: any;

    constructor(kind: ErrorKind, code: string, message: string, payload: any) {
        super(message);
        this.kind = kind;
        this.code = code;
        this.payload = payload;
    }
}

export class AggregatedError extends Error {
    errors: any[]

    constructor(errors: any[] = []) {
        super();
        this.errors = errors;
    }
}

