import { createContext } from "react";

const ClusterContext = createContext<ClusterContextValueType | any>({});

export type ClusterContextValueType = {
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

export default ClusterContext;