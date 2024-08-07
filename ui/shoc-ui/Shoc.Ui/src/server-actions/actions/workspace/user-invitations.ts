import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import UserInvitationsClient from '@/clients/shoc/workspace/user-invitations-client';

export const getAll = defineServerAction(({ }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserInvitationsClient).getAll(token)));
});

export const countAll = defineServerAction(({ }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserInvitationsClient).countAll(token)));
});

export const accept = defineServerAction(({ input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserInvitationsClient).accept(token, input)));
});

export const decline = defineServerAction(({ input }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(UserInvitationsClient).decline(token, input)));
});

export const serverActions = {
    'workspace/user-invitations/getAll': getAll,
    'workspace/user-invitations/countAll': countAll,
    'workspace/user-invitations/accept': accept,
    'workspace/user-invitations/decline': decline
}
