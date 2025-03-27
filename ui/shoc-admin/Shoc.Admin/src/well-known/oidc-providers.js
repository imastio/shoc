export const oidcProviderCodePattern = /^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,64}$/

export const oidcProviderNameMaxLength = 256;
export const oidcProviderClientIdMaxLength = 1024;
export const oidcProviderClientSecretMaxLength = 4096;
export const oidcProviderScopeMaxLength = 512;

export const oidcProviderTypes = [ 
    {
        key: 'local', 
        display: 'Local'
    },
    {
        key: 'google', 
        display: 'Google'
    },
    {
        key: 'facebook', 
        display: 'Facebook'
    },
    {
        key: 'okta', 
        display: 'Okta'
    },
    {
        key: 'other', 
        display: 'Other'
    }
]

export const oidcProviderTypesMap = Object.assign({}, ...oidcProviderTypes.map((entry) => ({[entry.key]: entry.display})));
