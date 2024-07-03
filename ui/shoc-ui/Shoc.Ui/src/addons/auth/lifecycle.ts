import { Account, Awaitable, DefaultSession, Profile, User } from "@auth/core/types";
import { AuthorizationCodeGrant } from "@/addons/oauth2/authorization-code-grant";
import { getClientId, getClientSecret, getIssuer, getOpenIdScopes, getRefreshRequestLifetime, getTokenCacheLifetime, getTokenExpirationSkew } from "./config";
import { JWT } from "next-auth/jwt";
import { Session } from "next-auth";
import { sessionAccessTokenCache, tokenRefreshRequests } from "./cache";
import { AdapterUser } from "@auth/core/adapters";

export const codeGrant = new AuthorizationCodeGrant({
    authority: getIssuer(),
    clientId: getClientId(),
    clientSecret: getClientSecret(),
    scope: getOpenIdScopes()
});

export function getProfileUser(profile: Profile): Awaitable<User>{
    return {
        id: profile.sub || undefined,
        name: profile.name,
        email: profile.email,
        emailVerified: Boolean(profile.emailVerified),
        username: profile.preferred_username,
        userType: profile.user_type as string
    }
}

export function sessionCallback({ session, token }: { session: Session, token: JWT }): Session {

    return {
        ...session,
        user: token?.error ? null : token.user,
        error: token?.error
    }
}

export async function jwtCallback({ token, account, user } : { token: JWT, account: Account | null, user: User | AdapterUser }): Promise<JWT> {
    // on the first call when account is available based on OIDC response save the tokens
    if (account) {  
        const sid = crypto.randomUUID();

        sessionAccessTokenCache.set(`${sid}.at`, account.access_token, 60);

        return {
            sid: sid,
            idToken: account.id_token,
            accessToken: account.access_token,
            expiresAt: account.expires_at || (Number(token.iat || 0) + Number(account.expires_in || 0)),
            refreshToken: account.refresh_token,
            user: user,
            error: null
          };         
    }
    
    // if no account available but token is not expired everything is ok
    if(Number(token.expiresAt || 0) + getTokenExpirationSkew() > Math.floor(Date.now() / 1000)){
        sessionAccessTokenCache.set(`${token.sid}.at`, token.accessToken, getTokenCacheLifetime());
        return token;
    }

    try {
       
        // use this key to send single request for (refresh, access) token pair
        const requestKey = `${token.refreshToken}_${token.accessToken}`

        let request;

        // if request with the key is already there use it
        // othwerwise build a new request
        if(tokenRefreshRequests.has(requestKey)){
            request = tokenRefreshRequests.get(requestKey)
        }
        else {
            request = codeGrant.refreshToken({
                refreshToken: token.refreshToken,
                accessToken: token.accessToken
            });

            // store in the cache
            tokenRefreshRequests.set(requestKey, request, getRefreshRequestLifetime());
        }
        console.log("Refresh request queue state", {
            keys: Object.keys(tokenRefreshRequests.innerCache.data)
        })

        // wait for the completion
        const refreshed = await request;

        // build the new token
        const newToken = {
            ...token,
            sid: token.sid,
            user: token.user,
            idToken: refreshed.getIdToken() || null,
            accessToken: refreshed.getAccessToken(),
            expiresAt: Math.floor(refreshed.getExpiredAt().getTime() / 1000),
            refreshToken: refreshed.getRefreshToken() || token.refreshToken,
            error: null
        }

        sessionAccessTokenCache.set(`${token.sid}.at`, token.accessToken, getTokenCacheLifetime());

        return newToken;
    }
    catch(error){
        sessionAccessTokenCache.del(`${token.sid}.at`);
        return { 
            ...token, 
            sid: token.sid,
            user: null,
            idToken: null,
            accessToken: null, 
            refreshToken: null, 
            error: "refresh_error" as const 
        }
    }
}