export const accessAreas = [ 
    {
        key: 'identity', 
        display: 'Identity and User Management'
    }
]
export const accessAreasMap = Object.assign({}, ...accessAreas.map((entry) => ({[entry.key]: entry.display})));
