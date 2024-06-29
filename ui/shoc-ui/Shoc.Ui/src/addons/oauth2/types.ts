export interface OAuth2ServerConfig {
    authority: string;
    clientId: string;
    clientSecret?: string;
    scope: string;
}

export interface TokenEndpointResponse {
    access_token: string;
    refresh_token?: string;
    id_token?: string;
    expires_in?: number;
    expires_at?: number | Date | string;
}

export interface TokenRequestHints {
    tokenEndpoint?: string;
}

export interface RefreshTokenParams {
    accessToken: string;
    refreshToken: string | null;
}
