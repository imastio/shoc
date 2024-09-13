export const genders = [ {key: 'male', display: 'Male'}, {key: 'female', display: 'Female'} ]

export const gendersMap = Object.assign({}, ...genders.map((entry) => ({[entry.key]: entry.display})));