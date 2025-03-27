import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";
import OidcProvidersTable from "./_components/oidc-providers-table";

export const metadata: Metadata = {
    title: 'Oidc Providers',
    description: 'Manage all OIDC-compatible providers for federated login.',
}

export default function OidcProvidersPage() {
    return (
        <PageContainer fluid title="Oidc Providers">
            <OidcProvidersTable />
        </PageContainer>
    )
} 

