import { IncomingHttpHeaders } from "http";

export function toHeaders(incomingHeaders: IncomingHttpHeaders): Headers {
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