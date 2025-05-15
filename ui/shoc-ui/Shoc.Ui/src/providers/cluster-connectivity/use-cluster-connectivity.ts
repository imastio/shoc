import { useContext } from "react";
import ClusterConnectivityContext, { ClusterConnectivityContextValueType } from "./cluster-connectivity-context";

export default function useClusterConnectivity(){
    return useContext<ClusterConnectivityContextValueType>(ClusterConnectivityContext);
}