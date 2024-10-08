import { ReactNode } from "react";
import WorkspaceClustersMenu from "./workspace-clusters-menu";

export default async function SingleWorkspaceClustersLayout({ params: { name }, children }: { children: ReactNode, params: any }) {
    return <div className="w-full flex flex-col">
        <WorkspaceClustersMenu name={name} />
        <div className="px-4 md:px-12 flex flex-col h-full">
            {children}
        </div>
    </div>
}