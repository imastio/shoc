import AccessGuardLayout from "./_components/access-guard-layout";
import ConsoleLayout from "./_components/console-layout";
import PrivateInitLayout from "./_components/private-init-layout";
import { getEffectiveAccesses } from "./cached-global-actions";
import AccessProvider from "@/providers/access-provider/access-provider";

export default async function PrivateRootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {

    const accesses = await getEffectiveAccesses();

    return (
        <AccessProvider accesses={new Set<string>(accesses.data || [])}>
            <PrivateInitLayout>
                <AccessGuardLayout>
                    <ConsoleLayout>
                        {children}
                    </ConsoleLayout>
                </AccessGuardLayout>
            </PrivateInitLayout>
        </AccessProvider>
    );
}
