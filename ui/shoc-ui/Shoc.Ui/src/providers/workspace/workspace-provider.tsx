"use client"

import { useMemo } from "react";
import WorkspaceContext from "./workspace-context";

export default function WorkspaceProvider({ children, workspace } : { children: React.ReactNode, workspace: any }){
    
    const value = useMemo(() => ({
        name: workspace.name,
        type: workspace.type,
        role: workspace.role
    }), [workspace])

    return <WorkspaceContext.Provider value={value}>
        {children}
    </WorkspaceContext.Provider>
}