"use client"

import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
  } from "@/components/ui/card"  
import useClusterConnectivity from "@/providers/cluster-connectivity/use-cluster-connectivity";
import useCluster from "@/providers/cluster/use-cluster";
import { useIntl } from "react-intl"
import useClusterNodes from "../_providers/cluster-nodes/use-cluster-nodes";

export default function NodesCpuSummaryCard(){

    const intl = useIntl();
    const { value: cluster } = useCluster();
    const { value: connectivity } = useClusterConnectivity();
    const { value: nodes } = useClusterNodes();

    return <Card>
    <CardHeader className="flex flex-col items-stretch space-y-0 border-b p-0 sm:flex-row">
      <div className="flex flex-1 flex-col justify-center gap-1 px-6 py-5 sm:py-6">
        <CardTitle>CPU Availability</CardTitle>
        <CardDescription>
          Showing total number of CPU across {connectivity.nodesCount} nodes
        </CardDescription>
      </div>
      <div className="flex">
      <button
      className="relative z-30 flex flex-1 flex-col justify-center gap-1 border-t px-6 py-4 text-left even:border-l data-[active=true]:bg-muted/50 sm:border-l sm:border-t-0 sm:px-8 sm:py-6"
    >
      <span className="text-xs text-muted-foreground">
        Allocatable
      </span>
      <span className="text-lg font-bold leading-none sm:text-3xl">
        {nodes?.length}
      </span>
    </button>
      </div>
    </CardHeader>
  </Card>

}