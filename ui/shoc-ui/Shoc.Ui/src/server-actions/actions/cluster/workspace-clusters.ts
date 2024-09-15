import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import WorkspaceClustersClient from '@/clients/shoc/cluster/workspace-clusters-client';

export const getAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(WorkspaceClustersClient).getAll(token, workspaceId)));
});

export const serverActions = {
    'cluster/workspace-clusters/getAll': getAll
}
