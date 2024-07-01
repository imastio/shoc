import { TokenEndpointResponse } from "./types";
import { decodeJwt, mapExpiresAt, mapExpiresIn } from "./utils";

const EXPIRATION_SKEW_SECONDS = 10;

export default class TokenResult {
    private accessToken: string;
    private refreshToken: string | null;
    private idToken: string | null;
    private expiresAt: Date;

    public constructor( {accessToken, refreshToken, idToken, expiresAt} : {accessToken: string, refreshToken: string | null, idToken: string | null, expiresAt: Date}){
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
        this.idToken = idToken;
        this.expiresAt = expiresAt;
    }

    public static fromResponse(response: TokenEndpointResponse){
        
        const accessToken = response.access_token;
        const refreshToken = response.refresh_token || '';
        const idToken = response.id_token || '';
        
        const decodedAccessToken = decodeJwt(response.access_token);

        let expiresAt = new Date();

        if(decodedAccessToken?.exp){
            expiresAt = mapExpiresAt(decodedAccessToken?.exp)
        }
        else if(response.expires_at){
            expiresAt = mapExpiresAt(response.expires_at);
        }
        else if(response.expires_in){
            expiresAt = mapExpiresIn(response.expires_in);
        }

        return new TokenResult({accessToken, refreshToken, idToken, expiresAt});
    }

    public getAccessToken(): string {
        return this.accessToken;
    }

    public getRefreshToken(): string {
        return this.refreshToken || '';
    }

    public getIdToken(): string {
        return this.idToken || '';
    }

    public isExpired(){
        return this.expiresAt.getTime() - Date.now() - (EXPIRATION_SKEW_SECONDS * 1000) <= 0;
    }

    public getExpiredAt(){
        return this.expiresAt;
    }

    public isRefreshable(){
        return this.refreshToken && this.refreshToken.length > 0;
    }
}