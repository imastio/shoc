import { createContext } from "react";

const ClusterNodesContext = createContext<ClusterNodesContextValueType | any>({});

export type ClusterNodesContextValueType = {
    value: any[],
    load: () => Promise<any>,
    loading: boolean,
    errors: any[]
}

export default ClusterNodesContext;