import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import { authenticatedUser } from '@/clients/authenticated';
import clientGuard from '@/clients/client-guard';
import CurrentUserClient from '@/clients/shoc/identity/current-user-client';

export const getEffectiveAccesses = defineServerAction(({ }) => {
    return authenticatedUser(token => clientGuard(() => shocClient(CurrentUserClient).getEffectiveAccesses(token)));
});

export const serverActions = {
    'identity/current-user/getAll': getEffectiveAccesses
}
