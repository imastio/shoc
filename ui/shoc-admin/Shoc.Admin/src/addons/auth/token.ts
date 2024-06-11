export async function refreshToken(
    refreshToken: string,
    options: { 
        issuer: string, 
        clientId: string, 
        clientSecret: string 
    }): Promise<any> {

    const issuerWithSlash = options.issuer.endsWith("/") ? options.issuer : `${options.issuer}/`;
    const metadataUrl = `${issuerWithSlash}.well-known/openid-configuration`
    const metadataResponse = await fetch(metadataUrl);
    const tokenUri = (await metadataResponse.json()).token_endpoint;

    const response = await fetch(tokenUri, {
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: new URLSearchParams({
            client_id: options.clientId,
            client_secret: options.clientSecret,
            grant_type: "refresh_token",
            refresh_token: refreshToken
        }),
        method: "POST",
    })

    const tokens = await response.json()

    if (!response.ok) {
        throw Error("Token was not refreshed");
    }

    return tokens;
}
