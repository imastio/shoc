import { useContext } from "react";
import ClusterAccessContext, { ClusterAccessContextValueType } from "./cluster-access-context";


export default function useClusterAccess(){
    return useContext<ClusterAccessContextValueType>(ClusterAccessContext);
}