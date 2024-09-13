import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";
import ApplicationsTable from "./_components/applications-table";

export const metadata: Metadata = {
    title: 'Applications',
    description: 'Manage all the thid-party applications in the system.',
}

export default function ApplicationsPage() {
    return (
        <PageContainer fluid title="Applications">
            <ApplicationsTable />
        </PageContainer>
    )
} 

