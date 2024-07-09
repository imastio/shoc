import { ReactNode } from "react";
import WorkspaceMembersMenu from "./workspace-members-menu";

export default async function SingleWorkspaceMembersLayout({ params: { name }, children }: { children: ReactNode, params: any }) {
    return <div className="w-full">
        <WorkspaceMembersMenu name={name} />
        <div className="px-4 md:px-12">
            {children}
        </div>
    </div>
}