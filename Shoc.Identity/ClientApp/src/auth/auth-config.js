const getAuthConfig = () => ({
    authority: process.env.REACT_APP_AUTH_AUTHORITY || "/", 
    clientId: process.env.REACT_APP_AUTH_CLIENT_ID || "identity",
    scope: process.env.REACT_APP_AUTH_SCOPE || "openid email profile",
    redirectUri: process.env.REACT_APP_AUTH_REDIRECT_URI || "/signed-in",
    silentRedirectUri: process.env.REACT_APP_AUTH_SILENT_REDIRECT_URI || "/signed-in-silent.html",
    postLogoutRedirectUri: process.env.REACT_APP_AUTH_POST_LOGOUT_REDURECT_URI || "/",
    storeKeyPrefix: process.env.REACT_APP_AUTH_STORE_KEY_PREFIX_TYPE || 'shcidt.',
    stateStorageType: process.env.REACT_APP_AUTH_STATE_STORAGE_TYPE || 'local',
    userStorageType: process.env.REACT_APP_AUTH_USER_STORAGE_TYPE || 'local',
});

export default getAuthConfig;
