import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import UserWorkspaceInvitationsClient from '@/clients/shoc/workspace/user-workspace-invitations-client';

export const getAll = defineServerAction(({ workspaceId }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspaceInvitationsClient).getAll(token, workspaceId)));
});

export const create = defineServerAction(({ workspaceId, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspaceInvitationsClient).create(token, workspaceId, input)));
});

export const updateById = defineServerAction(({ workspaceId, id, input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspaceInvitationsClient).updateById(token, workspaceId, id, input)));
});

export const deleteById = defineServerAction(({ workspaceId, id }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserWorkspaceInvitationsClient).deleteById(token, workspaceId, id)));
});

export const serverActions = {
    'workspace/user-workspace-invitations/getAll': getAll,
    'workspace/user-workspace-invitations/create': create,
    'workspace/user-workspace-invitations/updateById': updateById,
    'workspace/user-workspace-invitations/deleteById': deleteById
}
