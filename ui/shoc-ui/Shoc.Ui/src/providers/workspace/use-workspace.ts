import { useContext } from "react";
import WorkspaceContext, { WorkspaceContextValueType } from "./workspace-context";

export default function useWorkspace(){
    return useContext<WorkspaceContextValueType>(WorkspaceContext);
}