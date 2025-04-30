import { createContext } from "react";

const ClusterAccessContext = createContext<ClusterAccessContextValueType | any>({});

export type ClusterAccessContextValueType = {
    progress: boolean,
    accesses: Set<string>,
    hasAny: (requirements: string[]) => boolean,
    hasAll: (requirements: string[]) => boolean
}

export default ClusterAccessContext;