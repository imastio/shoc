"use client"

import Link from "next/link";
import { CommandIcon } from "lucide-react";
import { PublicFooterButtons } from "./public-footer-buttons";
import { useIntl } from "react-intl";
import staticLinks from "@/well-known/static-links";

export function PublicFooter() {
    const intl = useIntl();

    return (
        <footer className="border-t w-full h-16">
            <div className="container flex items-center sm:justify-between justify-center sm:gap-0 gap-4 h-full text-muted-foreground text-sm flex-wrap sm:py-0 py-3 max-sm:px-4">
                <div className="flex items-center gap-3">
                    <CommandIcon className="sm:block hidden w-5 h-5 text-muted-foreground" />
                    <p className="text-center">
                        {intl.formatMessage({id: 'global.builtBy'})} {" "}
                        <Link
                            className="px-1 underline underline-offset-2"
                            href={staticLinks.shocAuthorRepo}
                        >
                            davitp
                        </Link>
                        | {intl.formatMessage({id: 'global.footer.sourceNotice'})}{" "}
                        <Link
                            className="px-1 underline underline-offset-2"
                            href={staticLinks.githubRepo}
                        >
                            GitHub
                        </Link>
                        |
                    </p>
                </div>

                <div className="gap-4 items-center hidden md:flex">
                    <PublicFooterButtons />
                </div>
            </div>
        </footer>
    );
}
