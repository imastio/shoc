import { IntlMessageId } from "@/i18n/sources";

export const workspaceNamePattern = /^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$/

export const workspaceTypes: { key: string, display: IntlMessageId }[] = [ 
    {
        key: 'individual', 
        display: 'workspace.types.individual'
    },
    {
        key: 'organization', 
        display: 'workspace.types.organization'
    }
]

export const workspaceTypesMap = Object.assign({}, ...workspaceTypes.map((entry) => ({[entry.key]: entry.display})));;

export const workspaceStatuses: { key: string, display: IntlMessageId }[] = [ 
    {
        key: 'pending', 
        display: 'workspace.statuses.pending'
    },
    {
        key: 'active', 
        display: 'workspace.statuses.active'
    },
    {
        key: 'archived', 
        display: 'workspace.statuses.archived'
    }
]

export const workspaceStatusesMap = Object.assign({}, ...workspaceStatuses.map((entry) => ({[entry.key]: entry.display})));

export const workspaceRoles: { key: string, display: IntlMessageId }[] = [ 
    {
        key: 'owner', 
        display: 'workspace.roles.owner'
    },
    {
        key: 'admin', 
        display: 'workspace.roles.admin'
    },
    {
        key: 'member', 
        display: 'workspace.roles.member'
    },
    {
        key: 'guest', 
        display: 'workspace.roles.member'
    }
]

export const workspaceRolesMap = Object.assign({}, ...workspaceRoles.map((entry) => ({[entry.key]: entry.display})));
