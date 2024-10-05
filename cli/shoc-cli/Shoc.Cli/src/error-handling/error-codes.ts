const errors: Record<string, string> = {
    "UNKNOWN_ERROR": "Unknown Error",
    "NOT_FOUND_ERROR": "The requested object could not be found"
}

export function resolveError(code: string): string{
    return errors[code] || code;
}