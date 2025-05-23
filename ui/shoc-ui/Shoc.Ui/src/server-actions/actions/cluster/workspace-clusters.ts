import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import WorkspaceClustersClient from '@/clients/shoc/cluster/workspace-clusters-client';

export const getAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClustersClient).getAll(token, workspaceId)));
});

export const getByName = defineServerAction(({ workspaceId, name }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClustersClient).getByName(token, workspaceId, name)));
});

export const getPermissionsByName = defineServerAction(({ workspaceId, name }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClustersClient).getPermissionsByName(token, workspaceId, name)));
});

export const countAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClustersClient).countAll(token, workspaceId)));
});

export const ping = defineServerAction(({ workspaceId, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClustersClient).ping(token, workspaceId, input)));
});

export const create = defineServerAction(({ workspaceId, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClustersClient).create(token, workspaceId, input)));
});

export const updateById = defineServerAction(({ workspaceId, id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClustersClient).updateById(token, workspaceId, id, input)));
});

export const updateConfigurationById = defineServerAction(({ workspaceId, id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClustersClient).updateConfigurationById(token, workspaceId, id, input)));
});

export const serverActions = {
    'cluster/workspace-clusters/getAll': getAll,
    'cluster/workspace-clusters/getByName': getByName,
    'cluster/workspace-clusters/getPermissionsByName': getPermissionsByName,
    'cluster/workspace-clusters/countAll': countAll,
    'cluster/workspace-clusters/ping': ping,
    'cluster/workspace-clusters/create': create,
    'cluster/workspace-clusters/updateById': updateById,
    'cluster/workspace-clusters/updateConfigurationById': updateConfigurationById,
}
