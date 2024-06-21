
export function getClientId(){
    return process.env.SHOC_UI_NEXT_AUTH_CLIENT_ID || '';
}

export function getClientSecret(){
    return process.env.SHOC_UI_NEXT_AUTH_CLIENT_SECRET || '';
}

export function getIssuer(){
    return process.env.SHOC_UI_NEXT_AUTH_CLIENT_ISSUER || '';
}

export function getAuthSecret(){
    return process.env.SHOC_UI_NEXT_AUTH_SECRET || '';
}

export function getBaseUrl(){
    return process.env.SHOC_UI_BASE_URL || 'http://localhost:3000'
}
