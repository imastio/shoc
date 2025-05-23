import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { useIntl } from "react-intl";
import { cn } from "@/lib/utils";
import Link from "next/link";
import ClusterStatusBadge from "./cluster-status-badge";

interface ClusterCardProps {
  name: string,
  description: string,
  status: 'active' | 'archived',
  type: 'k8s'
}

export default function ClusterCard({ workspaceName, cluster, className }: { workspaceName: string, cluster: ClusterCardProps, className?: string }) {

  const intl = useIntl();
  const { name, description, status } = cluster;

  return (<Link prefetch={false} className={cn("h-full", className)} href={`/workspaces/${workspaceName}/clusters/${cluster.name}`}>
    <Card>
      <CardHeader className="flex flex-row justify-between items-center">
        <CardTitle className="flex items-center space-x-2 truncate">
          {name}
        </CardTitle>
        <div className="flex flex-row items-center justify-center space-x-2">
          <ClusterStatusBadge status={status} className="ml-2" />
        </div>
      </CardHeader>
      <CardContent className={cn("h-full text-sm text-muted-foreground text-balance line-clamp-1", description ? "" : "italic")}>
        {description || intl.formatMessage({ id: 'clusters.noDescription' })}
      </CardContent>
    </Card>
  </Link>

  )
}