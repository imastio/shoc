export const clusterNamePattern = /^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$/

export const clusterStatuses = [ 
    {
        key: 'active', 
        display: 'Active'
    },
    {
        key: 'archived', 
        display: 'Archived'
    }
]

export const clusterStatusesMap = Object.assign({}, ...clusterStatuses.map((entry) => ({[entry.key]: entry.display})));

export const clusterTypes = [ 
    {
        key: 'k8s', 
        display: 'Kubernetes'
    }
]

export const clusterTypesMap = Object.assign({}, ...clusterTypes.map((entry) => ({[entry.key]: entry.display})));
