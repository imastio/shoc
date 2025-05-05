import { ReactNode } from "react";
import { getByName } from "../../cached-workspace-actions";
import { getClusterByName, getClusterPermissionsByName } from "../cached-cluster-actions";
import ErrorScreen from "@/components/error/error-screen";
import ClusterAccessProvider from "@/providers/cluster-access/cluster-access-provider";

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

    const [cluster, permissions] = await Promise.all([getClusterByName(workspace.id, clusterName), getClusterPermissionsByName(workspace.id, clusterName)])

    if (cluster.errors || permissions.errors) {
        return <ErrorScreen errors={workspace.errors || permissions.errors} />
    }

    return <ClusterAccessProvider permissions={permissions.data || []}>
        <div className="h-full">
            {children}
        </div>
    </ClusterAccessProvider>
}