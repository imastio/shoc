const getAuthConfig = () => ({
    authority: process.env.REACT_APP_AUTH_AUTHORITY || "/", 
    clientId: process.env.REACT_APP_AUTH_CLIENT_ID || "identity",
    scope: process.env.REACT_APP_AUTH_SCOPE || "openid email profile",
    redirectUri: process.env.REACT_APP_AUTH_REDIRECT_URI || "/signed-in",
    silentRedirectUri: process.env.REACT_APP_AUTH_SILENT_REDIRECT_URI || "/signed-in-silent.html",
    postLogoutRedirectUri: process.env.REACT_APP_AUTH_POST_LOGOUT_REDURECT_URI || "/signed-out",
    storageType: process.env.REACT_APP_AUTH_STORAGE_TYPE || "session"
});

export default getAuthConfig;
