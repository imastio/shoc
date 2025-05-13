"use client"
import { createContext } from "react";

const WorkspaceContext = createContext<WorkspaceContextValueType | any>({});

export type WorkspaceContextValueType = {
    id: string,
    name: string,
    role: string,
    type: string
}

export default WorkspaceContext;