import { IncomingHttpHeaders } from "http";
import { JWT } from "next-auth/jwt";
import { getAuthSecret, getBaseUrl } from "./config";
import { getToken } from "@auth/core/jwt";

export async function getJwtNode(headersOverride: IncomingHttpHeaders): Promise<JWT  | null> {

    const secure = getBaseUrl().startsWith('https://')

    return await getToken({
        req: { headers: convertToHeaders(headersOverride) },
        secret: getAuthSecret(),
        secureCookie: secure,
        salt: `${secure ? '__Secure-' : ''}authjs.session-token`
    });
}

function convertToHeaders(incomingHeaders: IncomingHttpHeaders) {
    const headers = new Headers();

    for (const [key, value] of Object.entries(incomingHeaders)) {
      if (Array.isArray(value)) {
        value.forEach(val => headers.append(key, val));
      } else if (value !== undefined) {
        headers.append(key, value);
      }
    }
  
    return headers;
  }