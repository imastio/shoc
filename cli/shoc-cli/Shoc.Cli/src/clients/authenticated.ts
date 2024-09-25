import ClientCredentialsGrant from '@/addons/oauth2/client-credentials-grant';
import { AxiosError } from 'axios';
import 'server-only';

const TOKEN_TTL = 3600;
const DEFAULT_TTL = 3600;

const clientCache = new CacheStorage({
    prefix: 'client',
    defaultTtl: DEFAULT_TTL,
    checkEvery: 60
});

function config(){
    return {
        authority: process.env.ENL_SPACE_NEXT_AUTH_CLIENT_ISSUER || '',
        clientId: process.env.ENL_SPACE_NEXT_AUTH_API_CLIENT_ID || '',
        clientSecret: process.env.ENL_SPACE_NEXT_AUTH_API_CLIENT_SECRET,
        scope: 'openid offline_access'
    }
}

async function authenticatedImpl<TResult>(action: (token: string) => Promise<TResult>): Promise<TResult>{

    const conf = config();

    const clientCredentialsGrant = new ClientCredentialsGrant({
        authority: conf.authority,
        clientId: conf.clientId,
        clientSecret: conf.clientSecret,
        scope: conf.scope
    });

    const tokenSet = await clientCache.computeIfAbsent('tokenSet', () => clientCredentialsGrant.getToken());

    if(!tokenSet.isExpired()){
        return await action(tokenSet.getAccessToken());
    }

    clientCache.del('tokenSet');

    if(tokenSet.isRefreshable()){
        const result = await clientCredentialsGrant.refreshToken({
            accessToken: tokenSet.getAccessToken(),
            refreshToken: tokenSet.getRefreshToken()
        });
        clientCache.set('tokenSet', result, TOKEN_TTL);
        return await action(result.getAccessToken());
    }

    const fallback = await clientCache.computeIfAbsent('tokenSet', () => clientCredentialsGrant.getToken())
    return await action(fallback.getAccessToken());
}

async function authenticatedUserImpl<TResult>(action: (token: string) => Promise<TResult>): Promise<TResult>{

    await auth();
    const jwt = await getJwt();
    return await action(jwt?.actualAccessToken || '');
}

export async function authenticatedUser<TResult>(action: (token: string) => Promise<TResult>): Promise<TResult> {

    try {
        return await authenticatedUserImpl(action);
    }
    catch(error){
        if(error instanceof AxiosError){
            if(error.response?.status === 401){
                console.trace("Not authenticated with token", error.config?.headers?.Authorization)
            }

            throw error;            
        }
        throw ErrorDefinitions.notAuthenticated();
    }

}