import { AuthenticatedContext, AuthSession } from '@/core/types';
import keytar from 'keytar'
import { decodeJwt } from 'jose'
import { refresh } from './authorize';
import { getWellKnownEndpoints } from './context-resolver';

const ACCESS_TOKEN_KEY = 'shoc_access_token';
const REFRESH_TOKEN_KEY = 'shoc_refresh_token';

export async function storeSession(providerUrl: string, tokens: { accessToken: string, refreshToken: string }): Promise<void> {

    await keytar.setPassword(providerUrl, ACCESS_TOKEN_KEY, tokens.accessToken);
    await keytar.setPassword(providerUrl, REFRESH_TOKEN_KEY, tokens.refreshToken);
}

export async function checkSession(providerUrl: string): Promise<AuthSession | null> {

    const accessToken = await keytar.getPassword(providerUrl, ACCESS_TOKEN_KEY);

    if (!accessToken) {
        return null;
    }

    const payload = decodeJwt(accessToken);
    const expires = new Date((payload.exp || 0) * 1000);
    const expired = expires.getTime() < new Date().getTime()

    return {
        id: payload.sub || '',
        sub: payload.sub || '',
        email: payload['email'] as string,
        name: payload['name'] as string,
        username: payload['preferred_username'] as string,
        userType: payload['user_type'] as string,
        expires: expires,
        expired: expired
    }
}

export async function clearSession(providerUrl: string): Promise<void> {

    await keytar.deletePassword(providerUrl, ACCESS_TOKEN_KEY);
    await keytar.deletePassword(providerUrl, REFRESH_TOKEN_KEY);
}

export async function getAuthenticatedContext(providerUrl: URL): Promise<AuthenticatedContext> {

    const providerUrlString = providerUrl.toString();
    const session = await checkSession(providerUrlString);
    const accessToken = await keytar.getPassword(providerUrlString, ACCESS_TOKEN_KEY);
    const refreshToken = await keytar.getPassword(providerUrlString, REFRESH_TOKEN_KEY);

    if (!accessToken || !session) {
        throw Error('You need to authenticate to continue.')
    }

    if (!session.expired) {
        return {
            session: session,
            accessToken: accessToken,
            refreshToken: refreshToken,
        }
    }

    if (!refreshToken) {
        throw Error('Your session has expired. Please login again.')
    }

    const { idp } = await getWellKnownEndpoints(new URL(providerUrl));

    const refreshed = await refresh({ idp, accessToken, refreshToken })

    await storeSession(providerUrlString, { accessToken: refreshed.accessToken, refreshToken: refreshed.refreshToken || refreshToken })

    const newSession = await checkSession(providerUrlString);

    if(!newSession){
        throw Error('Could not extend your session, please login again.')
    }

    return {
        session: newSession,
        accessToken: refreshed.accessToken,
        refreshToken: refreshed.refreshToken || refreshToken,
    }
}
