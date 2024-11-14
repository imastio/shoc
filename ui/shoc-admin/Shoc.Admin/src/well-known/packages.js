export const packageScopes = [ 
    {
        key: 'user', 
        display: 'User'
    },
    {
        key: 'workspace', 
        display: 'Workspace'
    }
]

export const packageScopesMap = Object.assign({}, ...packageScopes.map((entry) => ({[entry.key]: entry.display})));
