import open from 'open';
import http from 'http';
import crypto from 'crypto';
import { ResolvedContext } from '@/core/types';
import { getFreePort } from '@/services/network';
import { logger } from '@/services/logger';

const AUTH_TIMEOUT_SECONDS = 5 * 60;

type TokenResult = {
    accessToken: string,
    refreshToken: string
}

type CodeResponse = {
    state: string | null,
    code: string | null
}

type OpenIdConfiguration = {
    authorizationEndpoint: string,
    tokenEndpoint: string
}

const authConfig = {
    clientId: 'cli',
    scope: 'openid email profile shoc offline_access'
}

export async function getOpenIdConfiguration(idp: URL): Promise<OpenIdConfiguration> {

    const wellKnownUrl = new URL(idp);
    wellKnownUrl.pathname = '/.well-known/openid-configuration'

    console.log("calling", wellKnownUrl.toString())
    try {
        const response = await (await fetch(wellKnownUrl)).json();
        return {
            authorizationEndpoint: response.authorization_endpoint,
            tokenEndpoint: response.token_endpoint
        }
    }
    catch (e) {
        const error = e as Error;
        console.log(error)
        throw Error(`Unable to obtain configuration to start authentication. Details: ${error?.message || 'Unknown Reason'}.`)
    }
}


export async function authorize({ idp }: { idp: URL }): Promise<TokenResult> {

    const port = await getFreePort();

    const openidConfiguration = await getOpenIdConfiguration(idp);

    const state = crypto.randomBytes(16).toString('hex');
    const codeVerifier = crypto.randomBytes(32).toString('hex');
    const codeChallenge = crypto.createHash('sha256').update(codeVerifier).digest('base64url');

    const redirectUri = `http://localhost:${port}/signed-in`;

    const authUrl = `${openidConfiguration.authorizationEndpoint}?${new URLSearchParams({
        response_type: 'code',
        client_id: authConfig.clientId,
        redirect_uri: redirectUri,
        scope: authConfig.scope,
        state: state,
        code_challenge: codeChallenge,
        code_challenge_method: 'S256'
    }).toString()}`;

    logger.info(`To complete authentication please login in the browser...`)

    await open(authUrl).catch(() => {
        logger.info(`If login page was not opened automatically, please use the following link to login: ${authUrl}`)
    });


    const code = await waitForCode({ port });

    if (code.state !== state) {
        throw Error('Authentication failed due to state mismatch');
    }

    if (!code.code) {
        throw Error('Authentication failed due to missing authorization code')
    }

    return await exchangeCode({
        tokenUrl: openidConfiguration.tokenEndpoint,
        code: code.code,
        codeVerifier,
        redirectUri: redirectUri
    })
}

async function waitForCode({ port }: { port: any }): Promise<CodeResponse> {

    const server = http.createServer();
    server.listen(port, () => {
        logger.info(`Waiting for the user to complete authentication in the browser...`);
    });

    const controller = new AbortController();

    try {
        return await Promise.race([
            codePromise(server),
            timeoutPromise(AUTH_TIMEOUT_SECONDS, controller)
        ]);
    }
    finally {
        controller.abort();
        server.close();
    }
}

function codePromise(server: http.Server): Promise<CodeResponse> {
    return new Promise((resolve) => {
        server.on('request', (req, res) => {

            if(!req.url?.includes('signed-in')){
                res.writeHead(404);
                res.end('Not found')
                return;
            }

            const query = new URL(req.url || '', `http://${req.headers.host}`).searchParams;
            const code = query.get('code');
            const state = query.get('state');

            resolve({ code, state })

            res.writeHead(200);
            res.end('Authentication is in progress. You can close the browser.')
        });
    })
}

function timeoutPromise(seconds: number, controller: AbortController): Promise<CodeResponse> {
    return new Promise((_, reject) => {
        const timeoutId = setTimeout(() => {
            reject(new Error(`Authentication was not completed within ${seconds / 60} minutes.`));
            controller.abort();
        }, seconds * 1000);

        controller.signal.addEventListener('abort', () => {
            clearTimeout(timeoutId);
          });
    });
}

async function exchangeCode({ tokenUrl, code, codeVerifier, redirectUri }: { tokenUrl: string, code: string, codeVerifier: string, redirectUri: string }): Promise<TokenResult> {

    const body = new URLSearchParams({
        grant_type: 'authorization_code',
        code: code,
        redirect_uri: redirectUri,
        client_id: authConfig.clientId,
        code_verifier: codeVerifier,
    });

    const response = await fetch(tokenUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: body.toString()
    });

    if (!response.ok) {
        throw new Error(`Authentication failed as no token was returned.`);
    }

    const result = await response.json();

    return {
        accessToken: result.access_token,
        refreshToken: result.refresh_token
    }
}