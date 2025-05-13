import { OAuth2ServerConfig, RefreshTokenParams, TokenEndpointResponse, TokenRequestHints } from "./types";
import TokenResult from "./token-result";

export default class BaseGrant {
    config: OAuth2ServerConfig;

    public constructor(config: OAuth2ServerConfig) {
        this.config = config;
    }

    public async getOpenidConfiguration(): Promise<{
        token_endpoint: string,
        authorization_endpoint: string,
        end_session_endpoint: string
    }> {
        const wellKnown = `${this.config.authority}/.well-known/openid-configuration`
        const result = await fetch(wellKnown)
        return await result.json();
    }

    public async refreshToken(token: RefreshTokenParams, hints?: TokenRequestHints): Promise<TokenResult> {

        if (!token.refreshToken) {
            throw new Error("No refresh token available!");
        }

        // get openid config
        const tokenEndpoint = hints?.tokenEndpoint || (await this.getOpenidConfiguration()).token_endpoint;

        // build token request
        const request = {
            grant_type: 'refresh_token',
            client_id: this.config.clientId,
            client_secret: this.config.clientSecret,
            refresh_token: token.refreshToken,
            access_token: token.accessToken,
        };

        const response = await fetch(tokenEndpoint, {
            method: 'POST',
            body: JSON.stringify(request),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        });

        return TokenResult.fromResponse(await response.json());
    }
}