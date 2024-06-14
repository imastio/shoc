
import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";
import UserGroupsTable from "./_components/user-groups-table";

export const metadata: Metadata = {
    title: 'User Groups',
    description: 'Manage all the user groups in the system.',
}

export default function UserGroupsPage() {
    return (
        <PageContainer fluid title="User Groups">
            <UserGroupsTable />
        </PageContainer>
    )
} 
