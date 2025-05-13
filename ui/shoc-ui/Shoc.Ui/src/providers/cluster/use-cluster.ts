import { useContext } from "react";
import ClusterContext, { ClusterContextValueType } from "./cluster-context";

export default function useCluster(){
    return useContext<ClusterContextValueType>(ClusterContext);
}