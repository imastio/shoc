import { useContext } from "react";
import ClusterNodesContext, { ClusterNodesContextValueType } from "./cluster-nodes-context";

export default function useClusterNodes(){
    return useContext<ClusterNodesContextValueType>(ClusterNodesContext);
}