export const workspaceNamePattern = /^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$/

export const workspaceTypes = [ 
    {
        key: 'individual', 
        display: 'Individual'
    },
    {
        key: 'organization', 
        display: 'Organization'
    }
]

export const workspaceTypesMap = Object.assign({}, ...workspaceTypes.map((entry) => ({[entry.key]: entry.display})));

export const workspaceStatuses = [ 
    {
        key: 'pending', 
        display: 'Pending'
    },
    {
        key: 'active', 
        display: 'Active'
    },
    {
        key: 'archived', 
        display: 'Archived'
    }
]

export const workspaceStatusesMap = Object.assign({}, ...workspaceStatuses.map((entry) => ({[entry.key]: entry.display})));

export const workspaceRoles = [ 
    {
        key: 'owner', 
        display: 'Owner'
    },
    {
        key: 'admin', 
        display: 'Admin'
    },
    {
        key: 'member', 
        display: 'Member'
    },
    {
        key: 'guest', 
        display: 'Guest'
    }
]

export const workspaceRolesMap = Object.assign({}, ...workspaceRoles.map((entry) => ({[entry.key]: entry.display})));
