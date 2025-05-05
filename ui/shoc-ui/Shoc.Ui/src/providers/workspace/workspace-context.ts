import { createContext } from "react";

const WorkspaceContext = createContext<WorkspaceContextValueType | any>({});

export type WorkspaceContextValueType = {
    name: string,
    role: string,
    type: string
    
}

export default WorkspaceContext;