const routeAccess: Record<string, any> = {
    "/": { oneOf: [] },
    "/dashboard": { oneOf: [] },
    "/myself": { oneOf: [] },
    "/myself/profile": { oneOf: [] },
    "/team": { oneOf: ['identity:users:list', 'identity:user_groups:list', 'identity:roles:list', 'identity:privileges:list'] },
    "/team/users": { oneOf: ['identity:users:list'] },
    "/team/users/:id": { oneOf: ['identity:users:read'] },
    "/team/groups": { oneOf: ['identity:user_groups:list'] },
    "/team/groups/:id": { oneOf: ['identity:user_groups:read'] },
    "/team/roles": { oneOf: ['identity:roles:list'] },
    "/team/roles/:id": { oneOf: ['identity:roles:read'] },
    "/team/privileges": { oneOf: ['identity:privileges:list'] },
    "/team/privileges/:id": { oneOf: ['identity:privileges:read'] },
}

export default routeAccess;

