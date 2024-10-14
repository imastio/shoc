export const registryNamePattern = /^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$/
export const registryNamespacePattern = /^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$/
export const registryHostPattern = /^(?:[a-zA-Z0-9.-]+(?:\.[a-zA-Z]{2,})?(?::\d{1,5})?)$/
export const maxDisplayNameLength = 64

export const registryStatuses = [ 
    {
        key: 'active', 
        display: 'Active'
    },
    {
        key: 'archived', 
        display: 'Archived'
    }
]

export const registryStatusesMap = Object.assign({}, ...registryStatuses.map((entry) => ({[entry.key]: entry.display})));

export const registryUsageScopes = [ 
    {
        key: 'global', 
        display: 'Global'
    },
    {
        key: 'workspace', 
        display: 'Workspace'
    }
]

export const registryUsageScopesMap = Object.assign({}, ...registryUsageScopes.map((entry) => ({[entry.key]: entry.display})));

export const registryProviderTypes = [ 
    {
        key: 'shoc', 
        display: 'Shoc'
    },
    {
        key: 'docker_hub', 
        display: 'Docker Hub'
    },
    {
        key: 'azure', 
        display: 'Azure'
    },
    {
        key: 'aws', 
        display: 'AWS'
    },
    {
        key: 'gcp', 
        display: 'GCP'
    },
    {
        key: 'github', 
        display: 'GitHub'
    }
]

export const registryProviderTypesMap = Object.assign({}, ...registryProviderTypes.map((entry) => ({[entry.key]: entry.display})));

export const registryCredentialSources = [ 
    {
        key: 'manual', 
        display: 'Manual'
    },
    {
        key: 'generated', 
        display: 'Generated'
    },
    {
        key: 'integration', 
        display: 'Integration'
    }
]

export const registryCredentialSourcesMap = Object.assign({}, ...registryCredentialSources.map((entry) => ({[entry.key]: entry.display})));

export const registrySigningKeyUsages = [ 
    {
        key: 'signing', 
        display: 'Signing'
    }
]

export const registrySigningKeyUsagesMap = Object.assign({}, ...registrySigningKeyUsages.map((entry) => ({[entry.key]: entry.display})));

export const registrySigningKeyAlgorithms = [ 
    {
        key: 'RS256', 
        display: 'RS256'
    },
    {
        key: 'ES256', 
        display: 'ES256'
    },
    {
        key: 'ES512', 
        display: 'ES512'
    }
]

export const registrySigningKeyAlgorithmsMap = Object.assign({}, ...registrySigningKeyAlgorithms.map((entry) => ({[entry.key]: entry.display})));
