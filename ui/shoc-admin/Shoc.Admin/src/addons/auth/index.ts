import NextAuth, { Profile } from "next-auth"
import DuendeIDS6Provider from "next-auth/providers/duende-identity-server6"
import { refreshToken } from "./token";
import { JWT } from "next-auth/jwt";
import { getToken } from "@auth/core/jwt";
import { headers } from "next/headers";
import { Awaitable, User } from "@auth/core/types";

function getClientId(){
    return process.env.SHOC_ADMIN_NEXT_AUTH_CLIENT_ID || '';
}

function getClientSecret(){
    return process.env.SHOC_ADMIN_NEXT_AUTH_CLIENT_SECRET || '';
}

function getIssuer(){
    return process.env.SHOC_ADMIN_NEXT_AUTH_CLIENT_ISSUER || '';
}

function getAuthSecret(){
    return process.env.SHOC_ADMIN_NEXT_AUTH_SECRET || '';
}

function getBaseUrl(){
    return process.env.SHOC_ADMIN_BASE_URL || 'http://localhost:3000'
}

function profileMapper(profile: Profile): Awaitable<User & any>{
    return {
        id: profile.sub || undefined,
        name: profile.name,
        email: profile.email,
        emailVerified: profile.emailVerified,
        username: profile.preferred_username,
        type: profile.user_type
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
            profile: profileMapper
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
                user: token?.error ? token.user as any : null,
                error: token?.error
            }

        },
        async jwt({ token, account, user, profile }) {
            // on the first call when account is available based on OIDC response save the tokens
            if (account) {    
                return {
                    access_token: account.access_token,
                    expires_at: account.expires_at,
                    refresh_token: account.refresh_token,
                    user: user,
                    error: null
                  };         
            }
            
            // if no account available but token is not expired everything is ok
            if(Date.now() < Number(token.expires_at || 0) * 1000){
                return token;
            }

            // otherwise token is expired
            try {
                const refreshed = await refreshToken(String(token.refresh_token || ''), {
                    clientId: getClientId(),
                    clientSecret: getClientSecret(),
                    issuer: getIssuer()
                });

                return {
                    ...token,
                    access_token: refreshed.access_token,
                    expires_at: Math.floor(Date.now() / 1000 + (Number(refreshed.expires_in) || 0)),
                    refresh_token: refreshed.refresh_token ?? token.refresh_token,
                    error: null
                }
            }
            catch(error){
                return { ...token, access_token: null, error: "refresh_error" as const }
            }
        }
    }
})

export async function getJwt(headersOverride?: Headers): Promise<JWT  | null> {

    const secure = getBaseUrl().startsWith('https://')

    return await getToken({
        req: { headers: headersOverride ?? headers() },
        secret: getAuthSecret(),
        secureCookie: secure,
        salt: `${secure ? '__Secure-' : ''}authjs.session-token`
    });

}