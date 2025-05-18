"use client"

import LoadingContainer from "@/components/general/loading-container";
import { Button } from "@/components/ui/button";
import useClusterConnectivity from "@/providers/cluster-connectivity/use-cluster-connectivity";
import useCluster from "@/providers/cluster/use-cluster"
import useWorkspace from "@/providers/workspace/use-workspace";
import { ReloadIcon } from "@radix-ui/react-icons";
import useClusterNodes from "../_providers/cluster-nodes/use-cluster-nodes";
import NodesCpuSummaryCard from "./nodes-cpu-summary-card";
import NodesMemorySummaryCard from "./nodes-memory-summary-card";
import NodesGpuSummaryCard from "./nodes-gpu-summary-card";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Terminal } from "lucide-react";
import ClusterConfigurationAlert from "./cluster-configuration-alert";
import NodesTableCard from "./nodes-table-card";

export default function SingleClusterClientPage() {

  const { value: workspace, loading: workspaceLoading } = useWorkspace();
  const { value: cluster, loading: clusterLoading } = useCluster();
  const { value: connectivity, loading: connectivityLoading } = useClusterConnectivity();

  const loading = workspaceLoading || clusterLoading || connectivityLoading;

  const { value: nodes, errors: nodesError, loading: nodesLoading } = useClusterNodes()

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