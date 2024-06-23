import axios, { AxiosResponse } from "axios";
import TokenResult from "./token-result";
import { OAuth2ServerConfig, TokenEndpointResponse, TokenRequestHints } from "./types";

export default class ClientCredentialsGrant {
    config: OAuth2ServerConfig;

    public constructor(config: OAuth2ServerConfig) {
        this.config = config;
    }

    public async getOpenidConfiguration(): Promise<any>{
        const wellKnown = `${this.config.authority}/.well-known/openid-configuration`
        return (await axios.get(wellKnown)).data;
    }

    public async getToken(hints?: TokenRequestHints): Promise<TokenResult> {

        // get openid config
        const tokenEndpoint = hints?.tokenEndpoint || (await this.getOpenidConfiguration()).token_endpoint;

        // build token request
        const request = {
            grant_type: 'client_credentials',
            client_id: this.config.clientId,
            client_secret: this.config.clientSecret,
            scope: this.config.scope
        };

        // request token set from the endpoint
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

    public async refreshToken(token: TokenResult, hints?: TokenRequestHints): Promise<TokenResult>{

        if(!token.getRefreshToken()){
            throw new Error("No refresh token available!");
        }

        // get openid config
        const tokenEndpoint = hints?.tokenEndpoint || (await this.getOpenidConfiguration()).token_endpoint;

        // build token request
        const request = {
            grant_type: 'refresh_token',
            client_id: this.config.clientId,
            client_secret: this.config.clientSecret,
            refresh_token: token.getRefreshToken(),
            access_token: token.getAccessToken(),
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