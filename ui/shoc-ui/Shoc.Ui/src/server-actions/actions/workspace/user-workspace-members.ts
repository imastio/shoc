import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import UserWorkspaceMembersClient from '@/clients/shoc/workspace/user-workspace-members-client';

export const getAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspaceMembersClient).getAll(token, workspaceId)));
});

export const updateById = defineServerAction(({ workspaceId, id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspaceMembersClient).updateById(token, workspaceId, id, input)));
});

export const deleteById = defineServerAction(({ workspaceId, id }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspaceMembersClient).deleteById(token, workspaceId, id)));
});

export const serverActions = {
    'workspace/user-workspace-members/getAll': getAll,
    'workspace/user-workspace-members/updateById': updateById,
    'workspace/user-workspace-members/deleteById': deleteById
}
