
import { Metadata } from "next";
import SingleOidcProviderClientPage from "./_components/single-oidc-provider-client-page";

export const metadata: Metadata = {
    title: 'Oidc Provider Details',
}

export default function SingleOidcProviderPage() {
    return <SingleOidcProviderClientPage />
}