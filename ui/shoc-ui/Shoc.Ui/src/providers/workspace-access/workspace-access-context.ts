import { createContext } from "react";

const WorkspaceAccessContext = createContext<WorkspaceAccessContextValueType | any>({});

export type WorkspaceAccessContextValueType = {
    progress: boolean,
    accesses: Set<string>,
    hasAny: (requirements: string[]) => boolean,
    hasAll: (requirements: string[]) => boolean
}

export default WorkspaceAccessContext;