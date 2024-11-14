
import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";
import RolesTable from "./_components/roles-table";

export const metadata: Metadata = {
    title: 'Roles',
    description: 'Manage all the roles in the system.',
}

export default function RolesPage() {
    return (
        <PageContainer fluid title="Roles">
            <RolesTable />
        </PageContainer>
    )
} 
