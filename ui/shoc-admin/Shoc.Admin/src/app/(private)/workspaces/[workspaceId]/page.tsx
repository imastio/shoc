import { Metadata } from "next";
import SingleWorkspaceClientPage from "./_components/single-workspace-client-page";

export const metadata: Metadata = {
    title: 'Workspace Details',
}

export default function SingleWorkspacePage() {
    return <SingleWorkspaceClientPage />
}