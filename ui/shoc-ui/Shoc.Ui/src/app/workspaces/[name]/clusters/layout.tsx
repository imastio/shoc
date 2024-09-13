import { ReactNode } from "react";
import WorkspaceClustersMenu from "./workspace-clusters-menu";

export default async function SingleWorkspaceClustersLayout({ params: { name }, children }: { children: ReactNode, params: any }) {
    return <div className="w-full">
        <WorkspaceClustersMenu name={name} />
        <div className="px-4 md:px-12">
            {children}
        </div>
    </div>
}