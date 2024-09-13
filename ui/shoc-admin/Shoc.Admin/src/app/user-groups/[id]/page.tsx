
import { Metadata } from "next";
import SingleUserGroupClientPage from "./_components/single-user-group-client-page";

export const metadata: Metadata = {
    title: 'User Group Details',
}

export default function SingleUserPage() {
    return <SingleUserGroupClientPage />
}