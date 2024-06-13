import { openidConfiguration } from "./well-known";

export async function refreshToken(
    refreshToken: string,
    accessToken: string,
    options: { 
        issuer: string, 
        clientId: string, 
        clientSecret: string 
    }): Promise<any> {

   const openIdConfig = await openidConfiguration(options.issuer);

    const response = await fetch(openIdConfig.token_endpoint, {
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: new URLSearchParams({
            client_id: options.clientId,
            client_secret: options.clientSecret,
            grant_type: "refresh_token",
            refresh_token: refreshToken,
            access_token: accessToken
        }),
        method: "POST",
    })

    const tokens = await response.json()

    if (!response.ok) {
        throw Error("Token was not refreshed");
    }

    return tokens;
}
