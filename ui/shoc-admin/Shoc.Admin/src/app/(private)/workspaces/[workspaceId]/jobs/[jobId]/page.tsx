import { Metadata } from "next";
import SingleJobClientPage from "./_components/single-job-client-page";

export const metadata: Metadata = {
    title: 'Job Details',
}

export default function SingleJobPage() {
    return <SingleJobClientPage />
}