import { TokenEndpointResponse } from "./types";
import { mapExpiresAt, mapExpiresIn } from "./utils";

const EXPIRATION_SKEW_SECONDS = 10;

export default class TokenResult {
    private accessToken: string;
    private refreshToken: string | null;
    private expiresAt: Date;

    private constructor(accessToken: string, refreshToken: string | null, expiresAt: Date){
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
        this.expiresAt = expiresAt;
    }

    public static fromResponse(response: TokenEndpointResponse){
        
        const accessToken = response.access_token;
        const refreshToken = response.refresh_token ?? '';
        
        let expiresAt = new Date();

        if(response.expires_at){
            expiresAt = mapExpiresAt(response.expires_at);
        }
        else if(response.expires_in){
            expiresAt = mapExpiresIn(response.expires_in);
        }

        return new TokenResult(accessToken, refreshToken, expiresAt);
    }

    public getAccessToken(): string {
        return this.accessToken;
    }

    public getRefreshToken(): string {
        return this.refreshToken || '';
    }

    public isExpired(){
        return this.expiresAt.getUTCMilliseconds() - (Date.now() + EXPIRATION_SKEW_SECONDS * 1000) <= 0;
    }

    public isRefreshable(){
        return this.refreshToken && this.refreshToken.length > 0;
    }
}