export async function openidConfiguration(issuer: string){

    const issuerWithSlash = issuer.endsWith("/") ? issuer : `${issuer}/`;
    const metadataUrl = `${issuerWithSlash}.well-known/openid-configuration`
    const metadataResponse = await fetch(metadataUrl);
    const result = await metadataResponse.json()

    const token_endpoint = result.token_endpoint;
    const authorization_endpoint = result.authorization_endpoint;
    const end_session_endpoint = result.end_session_endpoint;

    return {
        token_endpoint,
        authorization_endpoint,
        end_session_endpoint
    };
}