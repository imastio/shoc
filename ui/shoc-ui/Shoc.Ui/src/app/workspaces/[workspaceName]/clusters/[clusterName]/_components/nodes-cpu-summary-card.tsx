"use client"

import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import useClusterConnectivity from "@/providers/cluster-connectivity/use-cluster-connectivity";
import useCluster from "@/providers/cluster/use-cluster";
import { useIntl } from "react-intl"
import useClusterNodes from "../_providers/cluster-nodes/use-cluster-nodes";
import LoadingContainer from "@/components/general/loading-container";
import { cn } from "@/lib/utils";

export default function NodesCpuSummaryCard() {

  const intl = useIntl();
  const { value: cluster, loading: clusterLoading } = useCluster();
  const { value: connectivity, loading: connectivityLoading } = useClusterConnectivity();
  const { value: nodes, loading: nodesLoading } = useClusterNodes();

  return <LoadingContainer className="" loading={nodesLoading}>
    <Card>
      <CardHeader>
        <CardTitle>CPU Availability</CardTitle>
        <CardDescription>Showing CPU availability across all nodes</CardDescription>
      </CardHeader>
      <CardContent className="flex pb-0">
        <div className={cn("flex flex-row gap-4", connectivity.connected ? "" : "hidden")}>
          <div className="border-l pl-2 flex flex-col justify-center text-left">
            <span className="text-xs text-muted-foreground">Usage</span>
            <span className="text-lg font-semibold leading-none sm:text-3xl">1500 <span className="text-muted-foreground font-normal text-sm">15%</span></span>
          </div>
          <div className="border-l pl-2 flex flex-col justify-center text-left">
            <span className="text-xs text-muted-foreground">Allocatable</span>
            <span className="text-lg font-semibold leading-none sm:text-3xl">25,010</span>
          </div>
          <div className="border-l pl-2 flex flex-col justify-center text-left">
            <span className="text-xs text-muted-foreground">Capacity</span>
            <span className="text-lg font-semibold leading-none sm:text-3xl">25,010</span>
          </div>
        </div>
        <div className={cn("flex flex-1 justify-center", !connectivity.connected ? "" : "hidden")}>
          <h1 className="scroll-m-20 text-2xl font-semibold tracking-tight lg:text-3xl text-center">
            Not connected
          </h1>       
           </div>
      </CardContent>

    </Card>
  </LoadingContainer>
}