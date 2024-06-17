import NextAuth, { Profile } from "next-auth"
import DuendeIDS6Provider from "next-auth/providers/duende-identity-server6"
import { refreshToken } from "./token";
import { JWT } from "next-auth/jwt";
import { getToken } from "@auth/core/jwt";
import { headers } from "next/headers";
import { Awaitable, User } from "@auth/core/types";
import { getAuthSecret, getBaseUrl, getClientId, getClientSecret, getIssuer } from "./config";
import { openidConfiguration } from "./well-known";

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

export const { handlers, auth, signIn, signOut } = NextAuth({
    providers: [
        DuendeIDS6Provider({
            id: 'shoc',
            name: 'Shoc Identity',
            clientId: getClientId(),
            clientSecret: getClientSecret(),
            issuer: getIssuer(),
            profile: profileMapper,
            authorization: { params: { scope: 'openid email profile shoc offline_access' } }
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
                console.log("Just got account", account)
                return {
                    id_token: account.id_token,
                    access_token: account.access_token,
                    expires_at: account.expires_at,
                    refresh_token: account.refresh_token,
                    user: user,
                    error: null
                  };         
            }
            
            // if no account available but token is not expired everything is ok
            if(Date.now() < Number(token.expires_at || 0) * 1000){
                console.log("Token is ok", token)
                return token;
            }

            // otherwise token is expired
            try {
                const refreshed = await refreshToken(String(token.refresh_token || ''), String(token.access_token || ''), {
                    clientId: getClientId(),
                    clientSecret: getClientSecret(),
                    issuer: getIssuer()
                });
                console.log("Token refresh result", refreshed)
                return {
                    ...token,
                    id_token: refreshed.id_token || token.id_token || null,
                    access_token: refreshed.access_token,
                    expires_at: Math.floor(Date.now() / 1000 + (Number(refreshed.expires_in) || 0)),
                    refresh_token: refreshed.refresh_token ?? token.refresh_token,
                    error: null
                }
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
