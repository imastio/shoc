"use client"

import LoadingContainer from "@/components/general/loading-container";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import useClusterConnectivity from "@/providers/cluster-connectivity/use-cluster-connectivity";
import useCluster from "@/providers/cluster/use-cluster"
import useWorkspace from "@/providers/workspace/use-workspace";
import { rpc } from "@/server-actions/rpc";
import { ReloadIcon } from "@radix-ui/react-icons";
import { TrendingUp } from "lucide-react";
import { useCallback, useEffect } from "react";
import useClusterNodes from "../_providers/cluster-nodes/use-cluster-nodes";
import NodesCpuSummaryCard from "./nodes-cpu-summary-card";

export default function SingleClusterClientPage() {

  const { value: workspace, loading: workspaceLoading } = useWorkspace();
  const { value: cluster, loading: clusterLoading } = useCluster();
  const { value: connectivity, loading: connectivityLoading } = useClusterConnectivity();

  const loading = workspaceLoading || clusterLoading || connectivityLoading;

  const { value: nodes, errors: nodesError, loading: nodesLoading } = useClusterNodes()

  return <LoadingContainer loading={loading}>
    <div className="space-y-4">
      <pre>

      </pre>
      <div className="flex items-center justify-between space-y-2">
        <div className="flex flex-col">
          <h3 className="scroll-m-20 text-2xl font-semibold tracking-tight">
            {cluster.name}
          </h3>
          <p className="text-muted-foreground">
            {cluster.description}
          </p>
        </div>
        <Button variant='outline'>
          <ReloadIcon />
        </Button>
      </div>
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        <Card className="flex flex-col">
          <CardHeader className="items-center pb-0">
            <CardTitle>Nodes in cluster</CardTitle>
          </CardHeader>
          <CardContent className="flex-1 pb-0">
            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl text-center">
              {connectivity.nodesCount}
            </h1>
          </CardContent>
          <CardFooter className="flex-col gap-2 text-sm">
            <div className="flex items-center gap-2 font-medium leading-none">
              Trending up by 5.2% this month <TrendingUp className="h-4 w-4" />
            </div>
            <div className="leading-none text-muted-foreground">
              Showing total visitors for the last 6 months
            </div>
          </CardFooter>
        </Card>
      <NodesCpuSummaryCard />
      </div>
    </div>
  </LoadingContainer>
}