import { AuthSession } from '@/core/types';
import keytar from 'keytar'
import { decodeJwt } from 'jose'

const ACCESS_TOKEN_KEY = 'shoc_access_token';
const REFRESH_TOKEN_KEY = 'shoc_refresh_token';

export async function storeSession(providerUrl: string, tokens: { accessToken: string, refreshToken: string }) : Promise<void> {
    
    await keytar.setPassword(providerUrl, ACCESS_TOKEN_KEY, tokens.accessToken);
    await keytar.setPassword(providerUrl, REFRESH_TOKEN_KEY, tokens.refreshToken);
}

export async function checkSession(providerUrl: string) : Promise<AuthSession | null> {
    
    const accessToken = await keytar.getPassword(providerUrl, ACCESS_TOKEN_KEY);

    if(!accessToken){
        return null;
    }

    const payload = decodeJwt(accessToken);
    const expires = new Date((payload.exp || 0) * 1000);

    return {
        id: payload.sub || '',
        sub: payload.sub || '',
        email: payload['email'] as string,
        name: payload['name'] as string,
        username: payload['preferred_username'] as string,
        userType: payload['user_type'] as string,
        expires: expires
    }
}

export async function clearSession(providerUrl: string) : Promise<void> {
    
    await keytar.deletePassword(providerUrl, ACCESS_TOKEN_KEY);
    await keytar.deletePassword(providerUrl, REFRESH_TOKEN_KEY);
}
