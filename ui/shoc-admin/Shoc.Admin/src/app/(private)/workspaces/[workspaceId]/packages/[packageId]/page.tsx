import { Metadata } from "next";
import SinglePackageClientPage from "./_components/single-package-client-page";

export const metadata: Metadata = {
    title: 'Package Details',
}

export default function SinglePackagePage() {
    return <SinglePackageClientPage />
}