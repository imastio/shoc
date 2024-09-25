const errors: Record<string, string> = {
    "UNKNOWN_ERROR": "Unknown Error"
}

export function resolveError(code: string): string{
    return errors[code] || 'Unknown Error';
}