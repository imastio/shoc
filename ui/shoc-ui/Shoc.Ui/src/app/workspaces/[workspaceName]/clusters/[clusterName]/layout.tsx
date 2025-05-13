import { ReactNode } from "react";
import { getByName } from "../../cached-workspace-actions";
import { getClusterByName, getClusterPermissionsByName } from "../cached-cluster-actions";
import ErrorScreen from "@/components/error/error-screen";
import ClusterAccessProvider from "@/providers/cluster-access/cluster-access-provider";
import ClusterProvider from "@/providers/cluster/cluster-provider";

export default async function SingleClusterLayoutLayout(props: { children: ReactNode, params: Promise<any> }) {
    const params = await props.params;

    const {
        workspaceName,
        clusterName
    } = params;

    const {
        children
    } = props;

    const { data: workspace } = await getByName(workspaceName);

    if (workspace.errors) {
        return <ErrorScreen errors={workspace.errors} />
    }

    const [cluster, permissions] = await Promise.all([getClusterByName(workspace.id, clusterName), getClusterPermissionsByName(workspace.id, clusterName)])

    if (cluster.errors || permissions.errors) {
        return <ErrorScreen errors={cluster.errors || permissions.errors} />
    }

    return <ClusterProvider cluster={cluster.data}>
        <ClusterAccessProvider permissions={permissions.data || []}>
            {children}
        </ClusterAccessProvider>
    </ClusterProvider>
}
