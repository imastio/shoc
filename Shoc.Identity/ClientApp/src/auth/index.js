import getAuthConfig from "../auth/auth-config";
import { UserManager, WebStorageStateStore, User } from "oidc-client-ts";

const authConfig = getAuthConfig();

const getUser = () => {
    const oidcStorage = resolveStorage(authConfig.storageType).getItem(`oidc.user:${authConfig.authority}:${authConfig.clientId}`)

    if (!oidcStorage) {
        return null;
    }

    return User.fromStorageString(oidcStorage);
}

const resolveStorage = type => {

    switch (type) {
        case "local":
            return localStorage;
        default:
            return sessionStorage;
    }
}

const userManagerSettings = {
    authority: authConfig.authority,
    client_id: authConfig.clientId,
    redirect_uri: `${window.origin}${authConfig.redirectUri}`,
    silent_redirect_uri: `${window.origin}${authConfig.silentRedirectUri}`,
    post_logout_redirect_uri: `${window.origin}${authConfig.postLogoutRedirectUri}`,
    scope: authConfig.scope,
    userStore: new WebStorageStateStore({ store: resolveStorage(authConfig.storageType) }),
    stateStore: new WebStorageStateStore({ store: resolveStorage(authConfig.storageType) }),
    response_type: "code",
    monitorSession: true
};

const userManager = new UserManager(userManagerSettings);

const withToken = fn => fn(getUser()?.access_token);

export { withToken, userManager };

