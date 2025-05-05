import { SheetClose } from "@/components/ui/sheet";
import Anchor from "@/components/vc/anchor";

const NAVLINKS = [
    {
        title: "Docs",
        href: `/docs`,
    }
];

export function PublicNavMenu({ isSheet = false }) {
    return (
        <>
            {NAVLINKS.map((item) => {
                const Comp = (
                    <Anchor
                        key={item.title + item.href}
                        activeClassName="text-primary! md:font-semibold font-medium"
                        absolute
                        className="flex items-center gap-1 dark:text-stone-300/85 text-stone-800"
                        href={item.href}
                    >
                        {item.title}
                    </Anchor>
                );
                return isSheet ? (
                    <SheetClose key={item.title + item.href} asChild>
                        {Comp}
                    </SheetClose>
                ) : (
                    Comp
                );
            })}
        </>
    );
}