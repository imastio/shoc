
import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";
import RegistriesTable from "./_components/registries-table";

export const metadata: Metadata = {
    title: 'Registries',
    description: 'Manage all the registries in the system.',
}

export default function RegistriesPage() {
    return (
        <PageContainer fluid title="Registries">
            <RegistriesTable />
        </PageContainer>
    )
} 
