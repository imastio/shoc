import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { cn } from "@/lib/utils";
import useCluster from "@/providers/cluster/use-cluster";
import { Archive } from "lucide-react";
import { useIntl } from "react-intl";

export default function ClusterArchivedAlert() {

    const intl = useIntl();
    const { value: cluster } = useCluster()

    return <div className={cn(cluster?.status === 'archived' ? "flex" : "hidden")}>
        <Alert>
            <Archive className="h-4 w-4" />
            <AlertTitle>{intl.formatMessage({id: 'clusters.alerts.archived.title'})}</AlertTitle>
            <AlertDescription>
                {intl.formatMessage({id: 'clusters.alerts.archived.description'})}
            </AlertDescription>
        </Alert>
    </div>
}