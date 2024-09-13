import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import UserWorkspacesClient from '@/clients/shoc/workspace/user-workspaces-client';
import clientGuard from '@/clients/client-guard';

export const getAll = defineServerAction(({ }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspacesClient).getAll(token)));
});

export const getById = defineServerAction(({ id }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspacesClient).getById(token, id)));
});

export const getPermissionsById = defineServerAction(({ id }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspacesClient).getPermissionsById(token, id)));
});

export const getByName = defineServerAction(({ name }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspacesClient).getByName(token, name)));
});

export const getPermissionsByName = defineServerAction(({ name }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspacesClient).getPermissionsByName(token, name)));
});

export const create = defineServerAction(({ input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspacesClient).create(token, input)));
});

export const updateById = defineServerAction(({ id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspacesClient).updateById(token, id, input)));
});

export const deleteById = defineServerAction(({ id }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspacesClient).deleteById(token, id)));
});

export const serverActions = {
    'workspace/user-workspaces/getAll': getAll,
    'workspace/user-workspaces/getById': getById,
    'workspace/user-workspaces/getPermissionsById': getPermissionsById,
    'workspace/user-workspaces/getByName': getByName,
    'workspace/user-workspaces/getPermissionsByName': getPermissionsByName,
    'workspace/user-workspaces/create': create,
    'workspace/user-workspaces/updateById': updateById,
    'workspace/user-workspaces/deleteById': deleteById,
}
