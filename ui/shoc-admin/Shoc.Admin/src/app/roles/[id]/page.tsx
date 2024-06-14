
import { Metadata } from "next";
import SingleRoleClientPage from "./_components/single-role-client-page";

export const metadata: Metadata = {
    title: 'Role Details',
}

export default function SingleRolePage() {
    return <SingleRoleClientPage />
}