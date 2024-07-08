import { Account, Awaitable, Profile, User } from "@auth/core/types";
import { AuthorizationCodeGrant } from "@/addons/oauth2/authorization-code-grant";
import { getClientId, getClientSecret, getIssuer, getOpenIdScopes, getRefreshRequestLifetime, getTokenCacheLifetime, getTokenExpirationSkew } from "./config";
import { JWT } from "next-auth/jwt";
import { Session } from "next-auth";
import { sessionAccessTokenCache, tokenRefreshRequests } from "./cache";
import { AdapterUser } from "@auth/core/adapters";
import TokenResult from "../oauth2/token-result";

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
        sessionAccessTokenCache.set(`${sid}.at`, account.access_token, getTokenCacheLifetime());
        return {
            sid: sid,
            idToken: account.id_token,
            accessToken: account.access_token,
            expiresAt: account.expires_at || (Number(token.exp || 0) + Number(account.expires_in || 0)),
            refreshToken: account.refresh_token,
            user: user,
            error: null
          };         
    }
        
    // use this key to send single request for (refresh, access) token pair
    const requestKey = `${token.sid}`
    
    // see if the session is being refreshed right now
    const ongoingRequest = tokenRefreshRequests.get(requestKey);

    // if there is no ongoing request and there is a valid token just return
    if(!ongoingRequest && (token.expiresAt - getTokenExpirationSkew()) * 1000 > Date.now()){
        sessionAccessTokenCache.set(`${token.sid}.at`, token.accessToken, getTokenCacheLifetime());
        return token;
    }

    // if session is being wait for it's completion 
    if(ongoingRequest){
        return await handleRefreshResult(token, ongoingRequest);
    }

    // otherwise issue a new refresh request and wait for it's completion
    const newRequest = codeGrant.refreshToken({
        refreshToken: token.refreshToken,
        accessToken: token.accessToken
    });

    // store in the cache
    tokenRefreshRequests.set(requestKey, newRequest, getRefreshRequestLifetime());

    return await handleRefreshResult(token, newRequest);
}

const handleRefreshResult = async (current: JWT, request: Promise<TokenResult>) => {
    try {

        // wait for the completion
        const refreshed = await request;

        // build the new token
        const newToken = {
            ...current,
            sid: current.sid,
            user: current.user,
            idToken: refreshed.getIdToken() || null,
            accessToken: refreshed.getAccessToken(),
            expiresAt: Math.floor(refreshed.getExpiredAt().getTime() / 1000),
            refreshToken: refreshed.getRefreshToken() || current.refreshToken,
            error: null
        }

        sessionAccessTokenCache.set(`${current.sid}.at`, newToken.accessToken, getTokenCacheLifetime());

        return newToken;
    }
    catch(error){
        console.log("We've got an error while refreshing", error)
        sessionAccessTokenCache.del(`${current.sid}.at`);
        return { 
            ...current, 
            sid: current.sid,
            expiresAt: 0,
            user: null,
            idToken: null,
            accessToken: null, 
            refreshToken: null, 
            error: "refresh_error" as const 
        }
    }
}