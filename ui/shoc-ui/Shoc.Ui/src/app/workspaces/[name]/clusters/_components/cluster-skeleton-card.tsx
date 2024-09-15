"use client"

import { Skeleton } from "@/components/ui/skeleton";
import { cn } from "@/lib/utils";

export default function ClusterSkeletonCard({className}: {className?: string}) {
    return <div className={cn("w-full h-fit", className)}>
    <Skeleton className="h-[200px] w-full rounded-xl" />
  </div>
}