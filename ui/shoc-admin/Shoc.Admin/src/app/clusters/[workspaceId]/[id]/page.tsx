import { Metadata } from "next";
import SingleClusterClientPage from "./_components/single-cluster-client-page";

export const metadata: Metadata = {
    title: 'Cluster Details',
}

export default function SingleClusterPage() {
    return <SingleClusterClientPage />
}