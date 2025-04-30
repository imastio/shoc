import { ReactNode } from "react";
import WorkspaceSecretsMenu from "./workspace-secrets-menu";

export default async function SingleWorkspaceSecretsLayout({ params: { workspaceName }, children }: { children: ReactNode, params: any }) {
    return <div className="w-full">
        <WorkspaceSecretsMenu name={workspaceName} />
        <div className="px-4 md:px-12">
            {children}
        </div>
    </div>
}