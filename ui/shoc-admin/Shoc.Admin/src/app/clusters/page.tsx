
import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";
import ClustersTable from "./_components/clusters-table";

export const metadata: Metadata = {
    title: 'Clusters',
    description: 'Manage all the clusters in the system.',
}

export default function ClustersPage() {
    return (
        <PageContainer fluid title="Clusters">
            <ClustersTable />
        </PageContainer>
    )
} 
