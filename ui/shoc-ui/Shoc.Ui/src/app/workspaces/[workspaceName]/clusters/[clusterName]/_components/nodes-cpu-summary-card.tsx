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
import { Cpu } from "lucide-react";

export default function NodesCpuSummaryCard() {

  const intl = useIntl();
  const { summary, loading: nodesLoading } = useClusterNodes();

  return <LoadingContainer className="" loading={nodesLoading}>
    <Card className="h-full">
      <CardHeader>
        <CardTitle><span className="flex"><Cpu className="w-5 h-5 mr-2" /> {intl.formatMessage({ id: 'clusters.resources.cpu.title' })}</span></CardTitle>
        <CardDescription>{intl.formatMessage({ id: 'clusters.resources.cpu.description' })}</CardDescription>
      </CardHeader>
      <CardContent className="flex pb-0">
        <div className={cn("flex flex-row gap-4")}>
          <div className="border-l pl-2 flex flex-col justify-center text-left">
            <span className="text-xs text-muted-foreground">{intl.formatMessage({ id: 'clusters.resources.types.usage' })}</span>
            <span className="text-lg font-semibold leading-none xl:text-2xl">{summary?.cpu?.used ?? 'N/A'} <span className="text-muted-foreground font-normal text-sm"> {summary?.cpu?.allocatable ? Math.round((summary.cpu.used ?? 0) * 100 / summary.cpu.allocatable) : 'N/A'}% </span></span>
          </div>
          <div className="border-l pl-2 flex flex-col justify-center text-left">
            <span className="text-xs text-muted-foreground">{intl.formatMessage({ id: 'clusters.resources.types.allocatable' })}</span>
            <span className="text-lg font-semibold leading-none xl:text-2xl">{summary?.cpu?.allocatable ?? 'N/A'}</span>
          </div>
          <div className="border-l pl-2 flex flex-col justify-center text-left">
            <span className="text-xs text-muted-foreground">{intl.formatMessage({ id: 'clusters.resources.types.capacity' })}</span>
            <span className="text-lg font-semibold leading-none xl:text-2xl">{summary?.cpu?.capacity ?? 'N/A'}</span>
          </div>
        </div>
      </CardContent>
    </Card>
  </LoadingContainer>
}