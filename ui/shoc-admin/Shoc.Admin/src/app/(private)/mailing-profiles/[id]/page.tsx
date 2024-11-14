
import { Metadata } from "next";
import SingleMailingProfileClientPage from "./single-mailing-profile-client-page";

export const metadata: Metadata = {
    title: 'Mailing Profile',
}

export default function SingleMailingProfilePage() {
    return <SingleMailingProfileClientPage />
}