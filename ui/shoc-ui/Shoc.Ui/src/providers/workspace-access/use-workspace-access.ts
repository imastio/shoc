import { useContext } from "react";
import WorkspaceAccessContext, { WorkspaceAccessContextValueType } from "./workspace-access-context";

export default function useWorkspaceAccess(){
    return useContext<WorkspaceAccessContextValueType>(WorkspaceAccessContext);
}