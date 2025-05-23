"use client"

import { Badge } from "@/components/ui/badge";
import { Archive, CircleCheck } from "lucide-react";
import { useIntl } from "react-intl";

export default function ClusterStatusBadge({ status, className }: { status: 'active' | 'archived', className?: string }) {

    const intl = useIntl();

    return <Badge variant={status === 'active' ? 'outline' : 'secondary'} className={className}>
        {status === 'active' && <CircleCheck className="w-4 mr-2" />}
        {status === 'archived' && <Archive className="w-4 mr-2" />}
        {intl.formatMessage({ id: `clusters.statuses.${status}` })}
    </Badge>
}