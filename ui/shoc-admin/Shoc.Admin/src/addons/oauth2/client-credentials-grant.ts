import axios, { AxiosResponse } from "axios";
import TokenResult from "./token-result";
import { OAuth2ServerConfig, TokenEndpointResponse, TokenRequestHints } from "./types";
import BaseGrant from "./base-grant";

export default class ClientCredentialsGrant extends BaseGrant {

    public constructor(config: OAuth2ServerConfig) {
        super(config);
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

    
}