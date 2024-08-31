import { Metadata } from "next";
import SingleRegistryClientPage from "./_components/single-registry-client-page";

export const metadata: Metadata = {
    title: 'Registry Details',
}

export default function SingleRegistryPage() {
    return <SingleRegistryClientPage />
}