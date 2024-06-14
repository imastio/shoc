import { Metadata } from "next";
import UsersTable from "./_components/users-table";
import PageContainer from "@/components/general/page-container";

export const metadata: Metadata = {
    title: 'Users',
    description: 'Manage all the users in the system.',
}

export default function UsersPage() {
    return (
        <PageContainer fluid title="Users">
            <UsersTable />
        </PageContainer>
    )
} 
