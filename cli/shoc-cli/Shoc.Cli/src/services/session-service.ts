import keytar from 'keytar'

const ACCESS_TOKEN_KEY = 'shoc_access_token';
const REFRESH_TOKEN_KEY = 'shoc_refresh_token';

export async function storeSession(providerUrl: string, tokens: { accessToken: string, refreshToken: string }){
    keytar.setPassword(providerUrl, ACCESS_TOKEN_KEY, tokens.accessToken);
    keytar.setPassword(providerUrl, REFRESH_TOKEN_KEY, tokens.refreshToken);
}

