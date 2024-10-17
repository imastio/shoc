import { PublicFooter } from "./_components/public-footer";
import { PublicNavbar } from "./_components/public-navbar";

export default async function PublicLayout({ children }: any) {

    return <>
        <PublicNavbar />
        <main className="sm:container mx-auto w-[90vw] h-auto">
            {children}
        </main>
        <PublicFooter />
    </>
}