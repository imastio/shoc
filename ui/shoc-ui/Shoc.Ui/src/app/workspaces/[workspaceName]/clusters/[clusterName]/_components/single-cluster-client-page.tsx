"use client"

import LoadingContainer from "@/components/general/loading-container";
import useClusterConnectivity from "@/providers/cluster-connectivity/use-cluster-connectivity";
import useCluster from "@/providers/cluster/use-cluster"
import useWorkspace from "@/providers/workspace/use-workspace";
import NodesCpuSummaryCard from "./nodes-cpu-summary-card";
import NodesMemorySummaryCard from "./nodes-memory-summary-card";
import NodesGpuSummaryCard from "./nodes-gpu-summary-card";
import ClusterConfigurationAlert from "./cluster-configuration-alert";
import NodesTableCard from "./nodes-table-card";
import ClusterActionsDropdown, { ClusterActionTypes } from "./cluster-actions-dropdown";
import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Wand2 } from "lucide-react";
import { useIntl } from "react-intl";
import { ClusterPermissions } from "@/well-known/cluster-permissions";
import useClusterAccess from "@/providers/cluster-access/use-cluster-access";
import ClusterUpdateDialog from "./cluster-update-dialog";
import { useRouter } from "next/navigation";
import ClusterConfigurationUpdateDialog from "./cluster-configuration-update-dialog";
import ClusterStatusBadge from "../../_components/cluster-status-badge";
import ClusterArchivedAlert from "./cluster-archived-alert";

export default function SingleClusterClientPage() {

  const intl = useIntl();
  const router = useRouter();
  const { loading: workspaceLoading } = useWorkspace();
  const { value: cluster, load: loadCluster, loading: clusterLoading } = useCluster();
  const { value: connectivity, loading: connectivityLoading, load: loadConnectivity } = useClusterConnectivity();
  const { hasAll } = useClusterAccess();
  const [updatingItem, setUpdatingItem] = useState<any>(null)
  const [configuringItem, setConfiguringItem] = useState<any>(null)

  const loading = workspaceLoading || clusterLoading || connectivityLoading;

  const onActionSelected = async (action: ClusterActionTypes) => {

    if (action === 'refresh') {
      await loadCluster()
      await loadConnectivity()
    }

    if (action === 'update') {
      setUpdatingItem(cluster)
    }

    if (action === 'configure') {
      setConfiguringItem(cluster)
    }
  }

  return <div className="space-y-4">
    <ClusterUpdateDialog
      workspaceId={cluster.workspaceId}
      item={updatingItem}
      open={updatingItem}
      onClose={() => setUpdatingItem(null)} onSuccess={({ name }) => {
        loadCluster().then(() => router.replace(`/workspaces/${cluster.workspaceName}/clusters/${name}`))
      }}
    />
    <ClusterConfigurationUpdateDialog
      workspaceId={cluster.workspaceId}
      item={configuringItem}
      open={configuringItem}
      onClose={() => setConfiguringItem(null)} onSuccess={() => {
        loadConnectivity()
      }}
    />
    <LoadingContainer loading={loading}>
      <div className="flex items-center justify-between space-y-4">
        <div className="flex flex-col">
          <div className="flex flex-row space-x-2 items-center">
            <h3 className="scroll-m-20 text-2xl font-semibold tracking-tight">
              {cluster.name}
            </h3>
            <ClusterStatusBadge status={cluster.status} />
          </div>

          <p className="text-muted-foreground text-balanced">
            {cluster.description}
          </p>
        </div>
        <div className="space-x-2">
          {!connectivity.configured && <Button disabled={loading || !hasAll([ClusterPermissions.CLUSTER_UPDATE])} onClick={() => onActionSelected('configure')}>
            <Wand2 />
            <span className="sm:inline hidden">{intl.formatMessage({ id: 'global.actions.configure' })}</span>
          </Button>}
          <ClusterActionsDropdown disabled={loading} onSelect={onActionSelected} />
        </div>
      </div>
      <div className="flex flex-col space-y-4">
        <ClusterArchivedAlert />
        <ClusterConfigurationAlert />
      </div>

    </LoadingContainer>

    <div className="grid grid-cols-1 sm:grid-cols-1 lg:grid-cols-2 2xl:grid-cols-3 gap-4">
      <NodesCpuSummaryCard />
      <NodesMemorySummaryCard />
      <NodesGpuSummaryCard />
    </div>
    <div className="grid grid-cols-1">
      <NodesTableCard />
    </div>
  </div>
}