"use client"

import useCluster from "@/providers/cluster/use-cluster"
import useWorkspace from "@/providers/workspace/use-workspace";

export default function SingleClusterClientPage(){

    const { value: workspace } = useWorkspace();
    const { value: cluster } = useCluster();

    return  <pre>
    {JSON.stringify(cluster, null, 4)}
  </pre>
}