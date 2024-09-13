import BaseGrant from "./base-grant";
import { OAuth2ServerConfig } from "./types";

export class AuthorizationCodeGrant extends BaseGrant {
    public constructor(config: OAuth2ServerConfig) {
        super(config);
    }
}