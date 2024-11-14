
import { Metadata } from "next";
import SingleUserClientPage from "./_components/single-user-client-page";

export const metadata: Metadata = {
    title: 'User Profile',
}

export default function SingleUserPage() {
    return <SingleUserClientPage />
}