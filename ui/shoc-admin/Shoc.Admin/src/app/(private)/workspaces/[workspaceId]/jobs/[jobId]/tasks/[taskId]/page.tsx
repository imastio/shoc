import { Metadata } from "next";
import SingleJobTaskClientPage from "./_components/single-job-task-client-page";

export const metadata: Metadata = {
    title: 'Job Tak Details',
}

export default function SingleJobTasjPage() {
    return <SingleJobTaskClientPage />
}