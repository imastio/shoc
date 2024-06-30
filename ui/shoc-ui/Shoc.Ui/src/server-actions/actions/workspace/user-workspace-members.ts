import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import UserWorkspaceMembersClient from '@/clients/shoc/workspace/user-workspace-members-client';

export const getAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspaceMembersClient).getAll(token, workspaceId)));
});


export const serverActions = {
    'workspace/user-workspace-members/getAll': getAll,
}
