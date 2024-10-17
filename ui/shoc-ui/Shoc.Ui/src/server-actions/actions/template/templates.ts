import 'server-only';

import { defineServerAction } from '@/server-actions/define';
import { shocClient } from '@/clients/shoc';
import clientGuard from '@/clients/client-guard';
import TemplatesClient from '@/clients/shoc/package/templates-client';

export const getAll = defineServerAction(({  }) => {
    return clientGuard(() => shocClient(TemplatesClient).getAll());
});

export const getVariant = defineServerAction(({ name, variant }) => {
    return clientGuard(() => shocClient(TemplatesClient).getVariant(name, variant));
});

export const serverActions = {
    'template/templates/getAll': getAll,
    'template/templates/getVariant': getVariant
}
