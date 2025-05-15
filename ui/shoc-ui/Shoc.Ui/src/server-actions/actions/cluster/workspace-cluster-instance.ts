import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import WorkspaceClusterInstanceClient from '@/clients/shoc/cluster/workspace-cluster-instance-client';

export const getConnectivityById = defineServerAction(({ workspaceId, id }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClusterInstanceClient).getConnectivityById(token, workspaceId, id)));
});

export const getNodesById = defineServerAction(({ workspaceId, id }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClusterInstanceClient).getNodesById(token, workspaceId, id)));
});

export const serverActions = {
    'cluster/workspace-cluster-instance/getConnectivityById': getConnectivityById,
    'cluster/workspace-cluster-instance/getNodesById': getNodesById
}
