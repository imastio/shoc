import axios, { AxiosResponse } from "axios";
import { OAuth2ServerConfig, RefreshTokenParams, TokenEndpointResponse, TokenRequestHints } from "./types";
import TokenResult from "./token-result";

export default class BaseGrant {
    config: OAuth2ServerConfig;

    public constructor(config: OAuth2ServerConfig) {
        this.config = config;
    }

    public async getOpenidConfiguration(): Promise<any>{
        const wellKnown = `${this.config.authority}/.well-known/openid-configuration`
        return (await axios.get(wellKnown)).data;
    }

    public async refreshToken(token: RefreshTokenParams, hints?: TokenRequestHints): Promise<TokenResult>{

        if(!token.refreshToken){
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

        const response: TokenEndpointResponse = (await axios.post<any, AxiosResponse<TokenEndpointResponse>>(
            tokenEndpoint, 
            request, 
            {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }
        )).data;


        return TokenResult.fromResponse(response);
    }
}