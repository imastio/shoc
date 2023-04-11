import { WebStorageStateStore, User } from 'oidc-client-ts';
import getAuthConfig from './auth-config';
import resolveStorage from './utils';

const authConfig = getAuthConfig();

const userStore = resolveStorage(authConfig.userStorageType);
const stateStore = resolveStorage(authConfig.stateStorageType);

const oidcConfig = {
    authority: authConfig.authority,
    client_id: authConfig.clientId,
    redirect_uri: `${window.origin}${authConfig.redirectUri}`,
    silent_redirect_uri: `${window.origin}${authConfig.silentRedirectUri}`,
    post_logout_redirect_uri: `${window.origin}${authConfig.postLogoutRedirectUri}`,
    scope: authConfig.scope,
    userStore: new WebStorageStateStore({ prefix: authConfig.storeKeyPrefix, store: userStore }),
    stateStore: new WebStorageStateStore({ prefix: authConfig.storeKeyPrefix, store: stateStore }),
    response_type: 'code',
    staleStateAgeInSeconds: 3600,
    monitorSession: true
};

const oidcContextConfig = {
    ...oidcConfig,
    onSigninCallback: (user) => {
        window.location = user?.state;
    },
};

const getUser = () => {
    const userString = userStore.getItem(`${authConfig.storeKeyPrefix}user:${authConfig.authority}:${authConfig.clientId}`);

    if (!userString) {
        return null;
    }

    return User.fromStorageString(userString);
};

const withToken = (fn) => fn(getUser()?.access_token);

export { withToken, oidcConfig, oidcContextConfig };
