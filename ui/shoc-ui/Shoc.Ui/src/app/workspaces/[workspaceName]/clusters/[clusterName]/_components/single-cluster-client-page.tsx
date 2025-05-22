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

export default function SingleClusterClientPage() {
  
  const intl = useIntl();
  const { loading: workspaceLoading } = useWorkspace();
  const { value: cluster, load: loadCluster, loading: clusterLoading } = useCluster();
  const { value: connectivity, loading: connectivityLoading, load: loadConnectivity } = useClusterConnectivity();
  const [updating, setUpdating] = useState(false)
  const [configuring, setConfiguring] = useState(false)

  const loading = workspaceLoading || clusterLoading || connectivityLoading;

  const onActionSelected = async (action: ClusterActionTypes) => {

    if (action === 'refresh') {
      await loadCluster()
      await loadConnectivity()
    }

    if (action === 'update') {
      setUpdating(true)
    }

    if (action === 'configure') {
      setConfiguring(true)
    }

  }

  return <LoadingContainer loading={loading}>
    <div className="space-y-4">
      <div className="flex items-center justify-between space-y-2">
        <div className="flex flex-col">
          <h3 className="scroll-m-20 text-2xl font-semibold tracking-tight">
            {cluster.name}
          </h3>
          <p className="text-muted-foreground">
            {cluster.description}
          </p>
        </div>
        <div className="space-x-2">
          { !connectivity.configured &&  <Button disabled={loading} onClick={() => onActionSelected('configure')}>
            <Wand2 />
            <span className="sm:inline hidden">{intl.formatMessage({ id: 'global.actions.configure' })}</span>
          </Button>}
          <ClusterActionsDropdown disabled={loading} onSelect={onActionSelected} />
        </div>
      </div>
      <ClusterConfigurationAlert />
      <div className="grid grid-cols-1 sm:grid-cols-1 lg:grid-cols-2 2xl:grid-cols-3 gap-4">
        <NodesCpuSummaryCard />
        <NodesMemorySummaryCard />
        <NodesGpuSummaryCard />
      </div>
      <div className="grid grid-cols-1">
        <NodesTableCard />
      </div>
    </div>
  </LoadingContainer>
}