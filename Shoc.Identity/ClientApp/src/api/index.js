import getApiConfig from "./api-config";
import IdentityClient from "./identity-client";

// build configuration
const config = getApiConfig();

export const identityClient = new IdentityClient(config);
