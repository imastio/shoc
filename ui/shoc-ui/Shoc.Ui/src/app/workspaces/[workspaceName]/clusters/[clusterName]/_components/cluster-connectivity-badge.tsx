"use client"

import { Badge } from "@/components/ui/badge";
import { IntlMessageId } from "@/i18n/sources";
import { useIntl } from "react-intl";

const badgeVariants = {
    connected: {
        badgeClassName: 'border-green-600 text-green-600 dark:border-green-400 dark:text-green-400 [a&]:hover:bg-green-600/10 [a&]:hover:text-green-600/90 dark:[a&]:hover:bg-green-400/10 dark:[a&]:hover:text-green-400/90',
        dotClassName: 'size-1.5 rounded-full bg-green-600 dark:bg-green-400 mr-2',
        textTemplate: 'clusters.connectivity.connected' as IntlMessageId
    },
    disconnected: {
        badgeClassName: 'text-destructive [a&]:hover:bg-destructive/10 [a&]:hover:text-destructive/90 border-destructive',
        dotClassName: 'bg-destructive size-1.5 rounded-full mr-2',
        textTemplate: 'clusters.connectivity.disconnected' as IntlMessageId
    }
}

export default function ClusterConnectivityBadge({ connectivity }: { connectivity: any }) {

    const variantName = connectivity?.connected ? 'connected' : 'disconnected';
    const variant = badgeVariants[variantName];
    const intl = useIntl();

    return <Badge variant='outline' className={variant.badgeClassName}>
        <span className={variant.dotClassName} aria-hidden='true' />
        {intl.formatMessage({ id: variant.textTemplate })}
    </Badge>
}