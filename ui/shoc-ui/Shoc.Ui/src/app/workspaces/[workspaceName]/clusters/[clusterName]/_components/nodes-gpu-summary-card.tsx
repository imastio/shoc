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
import LoadingContainer from "@/components/general/loading-container";
import { cn } from "@/lib/utils";
import { Gpu } from "lucide-react";

export default function NodesGpuSummaryCard() {

  const intl = useIntl();
  const { summary, loading: nodesLoading } = useClusterNodes();

  return <LoadingContainer className="" loading={nodesLoading}>
    <Card className="h-full">
      <CardHeader>
        <CardTitle><span className="flex"><Gpu className="w-5 h-5 mr-2" /> {intl.formatMessage({ id: 'clusters.resources.nvidiaGpu.title' })}</span></CardTitle>
        <CardDescription>{intl.formatMessage({ id: 'clusters.resources.nvidiaGpu.description' })}</CardDescription>
      </CardHeader>
      <CardContent className="flex pb-0">
        <div className={cn("flex flex-row gap-4")}>
          <div className="border-l pl-2 flex flex-col justify-center text-left">
            <span className="text-xs text-muted-foreground">{intl.formatMessage({ id: 'clusters.resources.types.allocatable' })}</span>
            <span className="text-lg font-semibold leading-none xl:text-2xl">{summary?.nvidiaGpu?.allocatable ?? 'N/A'}</span>
          </div>
          <div className="border-l pl-2 flex flex-col justify-center text-left">
            <span className="text-xs text-muted-foreground">{intl.formatMessage({ id: 'clusters.resources.types.capacity' })}</span>
            <span className="text-lg font-semibold leading-none xl:text-2xl">{summary?.nvidiaGpu?.capacity ?? 'N/A'}</span>
          </div>
        </div>
      </CardContent>
    </Card>
  </LoadingContainer>
}