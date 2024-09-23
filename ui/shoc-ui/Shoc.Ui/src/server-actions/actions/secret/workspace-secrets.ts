import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import WorkspaceSecretsClient from '@/clients/shoc/secret/workspace-secrets-client';

export const getAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceSecretsClient).getAll(token, workspaceId)));
});

export const countAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceSecretsClient).countAll(token, workspaceId)));
});

export const create = defineServerAction(({ workspaceId, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceSecretsClient).create(token, workspaceId, input)));
});

export const updateById = defineServerAction(({ workspaceId, id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceSecretsClient).updateById(token, workspaceId, id, input)));
});

export const updateValueById = defineServerAction(({ workspaceId, id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceSecretsClient).updateValueById(token, workspaceId, id, input)));
});

export const deleteById = defineServerAction(({ workspaceId, id }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceSecretsClient).deleteById(token, workspaceId, id)));
});

export const serverActions = {
    'secret/workspace-secrets/getAll': getAll,
    'secret/workspace-secrets/countAll': countAll,
    'secret/workspace-secrets/create': create,
    'secret/workspace-secrets/updateById': updateById,
    'secret/workspace-secrets/updateValueById': updateValueById,
    'secret/workspace-secrets/deleteById': deleteById,
}
