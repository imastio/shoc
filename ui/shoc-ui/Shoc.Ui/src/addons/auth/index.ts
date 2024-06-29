import NextAuth, { Profile } from "next-auth"
import DuendeIDS6Provider from "next-auth/providers/duende-identity-server6"
import { JWT } from "next-auth/jwt";
import { getToken } from "@auth/core/jwt";
import { headers } from "next/headers";
import { Awaitable, User } from "@auth/core/types";
import { getAuthSecret, getBaseUrl, getClientId, getClientSecret, getIssuer } from "./config";
import { openidConfiguration } from "./well-known";
import { AuthorizationCodeGrant } from "../oauth2/authorization-code-grant";
import { cache } from "react";
import { decodeJwt } from "../oauth2/utils";

const TOKEN_EXPIRATION_BUFFER_SEC = 60;
const OPENID_SCOPES = 'openid email profile shoc offline_access';

function profileMapper(profile: Profile): Awaitable<User & any>{
    return {
        id: profile.sub || undefined,
        name: profile.name,
        email: profile.email,
        emailVerified: profile.emailVerified,
        username: profile.preferred_username,
        userType: profile.user_type
    }
}

export const codeGrant = new AuthorizationCodeGrant({
    authority: getIssuer(),
    clientId: getClientId(),
    clientSecret: getClientSecret(),
    scope: OPENID_SCOPES
});


export const { handlers, auth, signIn, signOut } = NextAuth({
    providers: [
        DuendeIDS6Provider({
            id: 'shoc',
            name: 'Shoc Identity',
            clientId: getClientId(),
            clientSecret: getClientSecret(),
            issuer: getIssuer(),
            profile: profileMapper,
            authorization: { params: { scope: OPENID_SCOPES } }
        })
    ],
    pages: {
        signIn: '/',
        signOut: '/',
        error: '/',
        verifyRequest: '/',
        newUser: '/'
    },
    trustHost: true,
    secret: getAuthSecret(),
    session: {
        strategy: 'jwt'
    },
    callbacks: {
        async session({ session, token }){

            session.user = token.user as any;
            return {
                ...session,
                user: token?.error ? null : token.user as any,
                error: token?.error
            }
        },
        async jwt({ token, account, user, profile }) {
            // on the first call when account is available based on OIDC response save the tokens
            if (account) {  
                return {
                    id_token: account.id_token,
                    access_token: account.access_token,
                    expires_at_sec: account.expires_at || (Number(token.iat || 0) + Number(account.expires_in || 0)),
                    refresh_token: account.refresh_token,
                    user: user,
                    error: null
                  };         
            }
            
            // if no account available but token is not expired everything is ok
            if(Number(token.expires_at_sec || 0) + TOKEN_EXPIRATION_BUFFER_SEC > Math.floor(Date.now() / 1000)){
                return token;
            }

            const details = {
                expiresAt: new Date(Number(token.expires_at_sec || 0) * 1000),
                accessTokenExpiresAt: new Date((decodeJwt(token.access_token as string)?.exp as number || 0) * 1000),
                now: new Date()
            }
            console.log("Expired Token, stamps", details)

            // otherwise token is expired
            try {
                console.log("Token expired, refreshing... ", new Date());
                const refreshed = await codeGrant.refreshToken({
                    refreshToken: token.refresh_token as string || '',
                    accessToken: token.access_token as string || ''
                })

                const newToken = {
                    ...token,
                    id_token: refreshed.getIdToken() || null,
                    access_token: refreshed.getAccessToken(),
                    expires_at_sec: Math.floor(refreshed.getExpiredAt().getTime() / 1000),
                    refresh_token: refreshed.getRefreshToken() || token.refresh_token,
                    error: null
                }
                return newToken;
            }
            catch(error){
                return { ...token, access_token: null, id_token: null, error: "refresh_error" as const }
            }
        }
    }
})

export async function singleSignOut({ postLogoutRedirectUri, state }: { postLogoutRedirectUri: string, state?: string }){
    const openIdConfig = await openidConfiguration(getIssuer());

    const jwt = await getJwt();

    const endSession = openIdConfig.end_session_endpoint;

    const urlParams = new URLSearchParams({ id_token_hint: String(jwt?.id_token || '') });

    if(jwt?.id_token){
        urlParams.set('id_token_hint', String(jwt?.id_token || ''))
    }

    if(postLogoutRedirectUri){
        urlParams.set('post_logout_redirect_uri', postLogoutRedirectUri);
    }

    if(state){
        urlParams.set('post_logout_redirect_uri', postLogoutRedirectUri);
    }

    return { redirectUri: `${endSession}?${urlParams.toString()}` }
}


export async function getJwt(headersOverride?: Headers): Promise<JWT | null> {

    const secure = getBaseUrl().startsWith('https://')

    return await getToken({
        req: { headers: headersOverride ?? headers() },
        secret: getAuthSecret(),
        secureCookie: secure,
        salt: `${secure ? '__Secure-' : ''}authjs.session-token`
    });
}
