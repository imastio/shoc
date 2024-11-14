
import { Metadata } from "next";
import SinglePrivilegeClientPage from "./_components/single-privilege-client-page";

export const metadata: Metadata = {
    title: 'Privilege Details',
}

export default function SinglePrivilegePage() {
    return <SinglePrivilegeClientPage />
}