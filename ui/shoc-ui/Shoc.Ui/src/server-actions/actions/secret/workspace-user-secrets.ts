import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import WorkspaceUserSecretsClient from '@/clients/shoc/secret/workspace-user-secrets-client';

export const getAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceUserSecretsClient).getAll(token, workspaceId)));
});

export const countAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceUserSecretsClient).countAll(token, workspaceId)));
});

export const create = defineServerAction(({ workspaceId, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceUserSecretsClient).create(token, workspaceId, input)));
});

export const updateById = defineServerAction(({ workspaceId, id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceUserSecretsClient).updateById(token, workspaceId, id, input)));
});

export const updateValueById = defineServerAction(({ workspaceId, id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceUserSecretsClient).updateValueById(token, workspaceId, id, input)));
});

export const deleteById = defineServerAction(({ workspaceId, id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceUserSecretsClient).deleteById(token, workspaceId, id)));
});

export const serverActions = {
    'secret/workspace-user-secrets/getAll': getAll,
    'secret/workspace-user-secrets/countAll': countAll,
    'secret/workspace-user-secrets/create': create,
    'secret/workspace-user-secrets/updateById': updateById,
    'secret/workspace-user-secrets/updateValueById': updateValueById,
    'secret/workspace-user-secrets/deleteById': deleteById,
}
