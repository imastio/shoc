import { ReactNode } from "react";
import WorkspaceSecretsMenu from "./workspace-secrets-menu";

export default async function SingleWorkspaceSecretsLayout({ params: { name }, children }: { children: ReactNode, params: any }) {
    return <div className="w-full">
        <WorkspaceSecretsMenu name={name} />
        <div className="px-4 md:px-12">
            {children}
        </div>
    </div>
}