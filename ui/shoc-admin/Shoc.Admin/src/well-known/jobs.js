export const labelNamePattern = /^[a-zA-Z][a-zA-Z0-9_-]{1,49}$/;

export const jobScopes = [ 
    {
        key: 'user', 
        display: 'User'
    },
    {
        key: 'workspace', 
        display: 'Workspace'
    }
]

export const jobScopesMap = Object.assign({}, ...jobScopes.map((entry) => ({[entry.key]: entry.display})));

export const jobStatuses = [ 
    {
        key: 'created', 
        display: 'Created'
    },
    {
        key: 'pending', 
        display: 'Pending'
    },
    {
        key: 'running', 
        display: 'Running'
    },
    {
        key: 'partially_succeeded', 
        display: 'Partially Succeeded'
    },
    {
        key: 'succeeded', 
        display: 'Succeeded'
    },
    {
        key: 'failed', 
        display: 'Failed'
    },
    {
        key: 'cancelled', 
        display: 'Cancelled'
    }
]

export const jobStatusesMap = Object.assign({}, ...jobStatuses.map((entry) => ({[entry.key]: entry.display})));
