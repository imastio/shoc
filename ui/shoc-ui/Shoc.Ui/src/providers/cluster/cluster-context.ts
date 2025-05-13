import { createContext } from "react";

const ClusterContext = createContext<ClusterContextValueType | any>({});

export type ClusterValueType = {
    id: string,
    workspaceId: string,
    workspaceName: string,
    name: string,
    description: string,
    type: string,
    status: string,
    created: string,
    updated: string
}

export type ClusterContextValueType = {
    value: ClusterValueType,
    initialValue: ClusterValueType,
    load: () => Promise<any>,
    loading: boolean,
    errors: any[]
}

export default ClusterContext;