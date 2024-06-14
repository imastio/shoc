const routeAccess: Record<string, any> = {
    "/": { oneOf: [] },
    "/dashboard": { oneOf: [] },
    "/myself": { oneOf: [] },
    "/myself/profile": { oneOf: [] },
    "/users": { oneOf: ['identity:users:list'] },
    "/users/[any]": { oneOf: ['identity:users:read'] },
    "/groups": { oneOf: ['identity:user_groups:list'] },
    "/groups/[any]": { oneOf: ['identity:user_groups:read'] },
    "/roles": { oneOf: ['identity:roles:list'] },
    "/roles/[any]": { oneOf: ['identity:roles:read'] },
    "/privileges": { oneOf: ['identity:privileges:list'] },
    "/privileges/[any]": { oneOf: ['identity:privileges:read'] },
}

function asRegex(original: string): string {

    let result: string = original;

    // replace slashes to escaped slashes
    result.replaceAll('/', '\\/')

    // replace [any] placeholders
    result = result.replaceAll('[any]', '[^\\/]+')

    // require strict match
    result = `^${result}$`

    return result;
}

const transformed: Record<string, any> = Object.fromEntries(Object.entries(routeAccess).map(entry => [asRegex(entry[0]), entry[1]]));
export default transformed;
