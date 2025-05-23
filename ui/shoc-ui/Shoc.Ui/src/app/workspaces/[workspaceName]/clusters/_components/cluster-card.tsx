import { Card, CardContent, CardFooter, CardHeader } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import KubernetesIcon from "@/components/icons/kubernetes-icon";
import ClusterIcon from "@/components/icons/cluster-icon";
import { useIntl } from "react-intl";
import { cn } from "@/lib/utils";
import Link from "next/link";

interface ClusterCardProps {
    name: string,
    description: string,
    status: 'active' | 'archived',
    type: 'k8s'
}

export default function ClusterCard({ workspaceName, cluster, className }: { workspaceName: string, cluster: ClusterCardProps, className?: string }) {

    const intl = useIntl();
    const { name, description, status, type } = cluster;

  return (
    <Card className={cn("h-fit", className)}>
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <div className="flex items-center space-x-2">
          { type === 'k8s' ? <KubernetesIcon className="h-6 w-6 text-primary" /> : <ClusterIcon className="h-6 w-6" /> }
          <h2 className="text-lg font-semibold truncate">{name}</h2>
        </div>
        <Badge variant={status === 'active' ? 'outline' : 'secondary'} className="ml-2">
          {intl.formatMessage({id: `clusters.statuses.${status}`})}
        </Badge>
      </CardHeader>
      <CardContent>
        <p className={cn("text-sm text-muted-foreground text-balance", description ? "" : "italic")}>
          { description || intl.formatMessage({id: 'clusters.noDescription'}) }
        </p>
      </CardContent>
      <CardFooter className="flex justify-between">
        <div className="flex space-x-2">
          <Link prefetch={false} href={`/workspaces/${workspaceName}/clusters/${cluster.name}`}>
          <Button variant="link" className="p-0">
            {intl.formatMessage({id: 'global.actions.view'})}
          </Button>
          </Link>
        </div>
      </CardFooter>
    </Card>
  )
}