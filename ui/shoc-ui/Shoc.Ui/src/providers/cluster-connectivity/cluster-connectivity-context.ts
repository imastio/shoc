import { createContext } from "react";

const ClusterConnectivityContext = createContext<ClusterConnectivityContextValueType | any>({});

export type ClusterConnectivityValueType = {
    id: string,
    workspaceId: string,
    configured: boolean,
    connected: boolean,
    message: string,
    nodesCount?: number,
    updated: string
}

export type ClusterConnectivityContextValueType = {
    value: ClusterConnectivityValueType,
    initialValue: ClusterConnectivityValueType,
    load: () => Promise<any>,
    loading: boolean,
    errors: any[]
}

export default ClusterConnectivityContext;