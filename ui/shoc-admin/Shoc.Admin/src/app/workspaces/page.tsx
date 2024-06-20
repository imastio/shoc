
import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";
import WorkspacesTable from "./_components/workspaces-table";

export const metadata: Metadata = {
    title: 'Workspaces',
    description: 'Manage all the workspaces in the system.',
}

export default function WorkspacesPage() {
    return (
        <PageContainer fluid title="Workspaces">
            <WorkspacesTable />
        </PageContainer>
    )
} 
