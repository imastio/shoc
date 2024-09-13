"use client"

import { Skeleton } from "@/components/ui/skeleton";

export default function WorkspaceSkeletonCard() {
    return <div className="flex flex-col space-y-3">
    <Skeleton className="h-[100px] w-full rounded-xl" />
  </div>
}