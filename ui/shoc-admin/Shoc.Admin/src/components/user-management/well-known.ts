export const accessAreas = [ 
    {
        key: 'identity', 
        display: 'Identity and User Management'
    },
    {
        key: 'settings', 
        display: 'Settings'
    }
]
export const accessAreasMap = Object.assign({}, ...accessAreas.map((entry) => ({[entry.key]: entry.display})));
