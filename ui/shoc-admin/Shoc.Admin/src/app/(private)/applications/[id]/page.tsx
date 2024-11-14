
import { Metadata } from "next";
import SingleApplicationClientPage from "./_components/single-application-client-page";

export const metadata: Metadata = {
    title: 'Application Details',
}

export default function SingleApplicationPage() {
    return <SingleApplicationClientPage />
}