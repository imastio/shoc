import { ReactNode } from "react";
import WorkspaceJobsMenu from "./workspace-jobs-menu";

export default async function SingleWorkspaceJobsLayout({ params: { name }, children }: { children: ReactNode, params: any }) {
    return <div className="w-full flex flex-col">
        <WorkspaceJobsMenu name={name} />
        <div className="px-4 md:px-12 flex flex-col h-full">
            {children}
        </div>
    </div>
}