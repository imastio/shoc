import { JWT, getToken } from "next-auth/jwt";
import { getAuthSecret, getBaseUrl } from "./config";
import { codeGrant } from "./lifecycle";
import { headers } from "next/headers";
import { IncomingHttpHeaders } from "http";
import { toHeaders } from "./utils";

export async function getJwt(headersOverride?: Headers): Promise<JWT| null> {

    const secure = getBaseUrl().startsWith('https://')

    return await getToken({
        req: { headers: headersOverride ?? headers() },
        secret: getAuthSecret(),
        secureCookie: secure,
        salt: `${secure ? '__Secure-' : ''}authjs.session-token`
    });
}

export async function getJwtNode(headersOverride: IncomingHttpHeaders): Promise<JWT  | null> {

    const secure = getBaseUrl().startsWith('https://')

    return await getToken({
        req: { headers: toHeaders(headersOverride) },
        secret: getAuthSecret(),
        secureCookie: secure,
        salt: `${secure ? '__Secure-' : ''}authjs.session-token`
    });
}

export async function singleSignOut({ postLogoutRedirectUri, state }: { postLogoutRedirectUri: string, state?: string }) {
    const openIdConfig = await codeGrant.getOpenidConfiguration();

    const jwt = await getJwt();

    const endSession = openIdConfig.end_session_endpoint;

    const urlParams = new URLSearchParams({ id_token_hint: String(jwt?.id_token || '') });

    if (jwt?.id_token) {
        urlParams.set('id_token_hint', String(jwt?.id_token || ''))
    }

    if (postLogoutRedirectUri) {
        urlParams.set('post_logout_redirect_uri', postLogoutRedirectUri);
    }

    if (state) {
        urlParams.set('post_logout_redirect_uri', postLogoutRedirectUri);
    }

    return { redirectUri: `${endSession}?${urlParams.toString()}` }
}