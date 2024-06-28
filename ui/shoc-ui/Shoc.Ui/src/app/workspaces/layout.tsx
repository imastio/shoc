import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import PrivateInitLayout from "../_components/private-init-layout";

export async function generateMetadata(): Promise<Metadata> {

    const intl = await getIntl();

    return {
        title: {
            template: `%s | ${intl.formatMessage({ id: 'shoc.platform' })}`,
            default: intl.formatMessage({ id: 'shoc.platform' }),
        },
    }
}

export default function WorkspacesLayout({ children }: any) {
    return <PrivateInitLayout>
        <div className="grid min-h-screen w-full">
            <div className="flex flex-col w-full">
                {children}
            </div>
        </div>
        </PrivateInitLayout>

}