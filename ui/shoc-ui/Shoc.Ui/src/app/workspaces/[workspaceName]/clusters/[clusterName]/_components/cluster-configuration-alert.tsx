import { Alert, AlertTitle } from "@/components/ui/alert";
import useClusterConnectivity from "@/providers/cluster-connectivity/use-cluster-connectivity";
import useCluster from "@/providers/cluster/use-cluster";
import { Check, ServerCrash, Wand } from "lucide-react";
import { useMemo } from "react";
import { useIntl } from "react-intl";

type ClusterConfigurationCondition = 'configured_connected' | 'configured_disconnected' | 'unconfigured';


export default function ClusterConfigurationAlert() {

    const intl = useIntl();
    const { value: cluster } = useCluster();
    const { value: connectivity } = useClusterConnectivity();

    const alerts: Record<ClusterConfigurationCondition, any> = useMemo(() => ({
        'configured_connected': {
            logo: Check,
            className: 'text-green-800 rounded-lg bg-green-50 dark:bg-gray-800 dark:text-green-400',
            title: intl.formatMessage({ id: 'clusters.connectivity.conditions.configured_connected.title' }),
            description: intl.formatMessage({ id: 'clusters.connectivity.conditions.configured_connected.description' }, { name: cluster.name })
        },
        'configured_disconnected': {
            logo: ServerCrash,
            className: 'text-red-800 rounded-lg bg-red-50 dark:bg-gray-800 dark:text-red-400',
            title: intl.formatMessage({ id: 'clusters.connectivity.conditions.configured_disconnected.title' }),
            description: intl.formatMessage({ id: 'clusters.connectivity.conditions.configured_disconnected.description' })
        },
        'unconfigured': {
            logo: Wand,
            className: 'text-violet-800 rounded-lg bg-violet-50 dark:bg-gray-800 dark:text-violet-400',
            title: intl.formatMessage({ id: 'clusters.connectivity.conditions.unconfigured.title' }),
            description: intl.formatMessage({ id: 'clusters.connectivity.conditions.unconfigured.description' })
        }
    }), [intl, cluster.name]);

    const condition: ClusterConfigurationCondition = useMemo(() => {
        if (connectivity.configured && connectivity.connected) {
            return 'configured_connected'
        }

        if (connectivity.configured && !connectivity.connected) {
            return 'configured_disconnected'
        }

        return 'unconfigured';
    }, [connectivity])

    const alert = useMemo(() => alerts[condition], [alerts, condition]);

    return <Alert variant='default' className={alert.className}>
        <alert.logo className="h-4 w-4" />
        <AlertTitle>{alert.title}</AlertTitle>
        <div className="col-start-2 grid justify-items-start gap-1 text-sm [&_p]:leading-relaxed">
            {alert.description}
        </div>
    </Alert>
}