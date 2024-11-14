
import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";
import PrivilegesTable from "./_components/privileges-table";

export const metadata: Metadata = {
    title: 'Privileges',
    description: 'Manage all the privileges in the system.',
}

export default function PrivilegesPage() {
    return (
        <PageContainer fluid title="Privileges">
            <PrivilegesTable />
        </PageContainer>
    )
} 
