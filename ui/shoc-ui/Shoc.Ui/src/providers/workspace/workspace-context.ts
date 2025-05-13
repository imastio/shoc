"use client"
import { createContext } from "react";

const WorkspaceContext = createContext<WorkspaceContextValueType | any>({});

export type WorkspaceValueType = {
    id: string,
    name: string,
    role: string,
    type: string
}

export type WorkspaceContextValueType = {
    value: WorkspaceValueType,
    initialValue: WorkspaceValueType,
    load: () => Promise<any>,
    loading: boolean,
    errors: any[]
}

export default WorkspaceContext;