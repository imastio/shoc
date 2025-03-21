export const accessAreas = [ 
    {
        key: 'identity', 
        display: 'Identity and User Management'
    },
    {
        key: 'workspace', 
        display: 'Workspace Management'
    },
    {
        key: 'registry', 
        display: 'Registry Management'
    },
    {
        key: 'cluster', 
        display: 'Cluster Management'
    },
    {
        key: 'secret', 
        display: 'Secret Management'
    },
    {
        key: 'package', 
        display: 'Package Management'
    },
    {
        key: 'job', 
        display: 'Job Management'
    },
    {
        key: 'settings', 
        display: 'Settings'
    }
]
export const accessAreasMap = Object.assign({}, ...accessAreas.map((entry) => ({[entry.key]: entry.display})));
