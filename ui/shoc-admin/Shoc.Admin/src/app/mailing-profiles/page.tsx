import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";
import MailingProfilesTable from "./_components/mailing-profiles-table";

export const metadata: Metadata = {
    title: 'Mailing Profiles',
    description: 'Manage all the profiles used for mailing.',
}

export default function MailingProfilesPage() {
    return (
        <PageContainer fluid title="Mailing Profiles">
            <MailingProfilesTable />
        </PageContainer>
    )
} 

