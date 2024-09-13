export const applicationProtocolTypes = [ 
    {
        key: 'oidc', 
        display: 'OpenID Connect'
    }
]
export const applicationProtocolTypesMap = Object.assign({}, ...applicationProtocolTypes.map((entry) => ({[entry.key]: entry.display})));

export const tokenUsageTypes = [ 
    {
        key: 'one_time', 
        display: 'One time'
    },
    {
        key: 'reuse', 
        display: 'Reuse'
    }
]
export const tokenUsageTypesMap = Object.assign({}, ...tokenUsageTypes.map((entry) => ({[entry.key]: entry.display})));

export const tokenExpirations = [ 
    {
        key: 'absolute', 
        display: 'Absolute'
    },
    {
        key: 'sliding', 
        display: 'Sliding'
    }
]
export const tokenExpirationsMap = Object.assign({}, ...tokenExpirations.map((entry) => ({[entry.key]: entry.display})));


export const applicationSecretTypes = [ 
    {
        key: 'shared_secret', 
        display: 'Shared Secret'
    },
    {
        key: 'cert_thumbprint', 
        display: 'Certificate Thumbprint'
    },
    {
        key: 'cert_name', 
        display: 'Certificate Name'
    },
    {
        key: 'cert_base64', 
        display: 'Certificate Base64'
    },
    {
        key: 'jwk', 
        display: 'JSON Web Key'
    }
]
export const applicationSecretTypesMap = Object.assign({}, ...applicationSecretTypes.map((entry) => ({[entry.key]: entry.display})));

export const applicationUriTypes = [ 
    {
        key: 'redirect_uri', 
        display: 'Redirect Uri'
    },
    {
        key: 'post_logout_redirect_uri', 
        display: 'Post Logout Redirect Uri'
    },
    {
        key: 'origin_uri', 
        display: 'Origin Uri'
    }
]
export const applicationUriTypesMap = Object.assign({}, ...applicationUriTypes.map((entry) => ({[entry.key]: entry.display})));
