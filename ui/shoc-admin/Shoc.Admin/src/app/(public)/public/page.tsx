
import { Metadata } from "next";
import PageContainer from "@/components/general/page-container";

export const metadata: Metadata = {
    title: 'Public Page',
    description: 'This is a public page',
}

export default function WorkspacesPage() {
    return (
        <PageContainer fluid title="Public Page">
            <p>This is a public page</p>
        </PageContainer>
    )
} 
