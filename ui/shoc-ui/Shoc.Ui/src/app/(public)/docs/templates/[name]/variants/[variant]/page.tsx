import { buttonVariants } from "@/components/ui/button";
import { ArrowLeftIcon } from "lucide-react";
import Link from "next/link";
import Typography from "@/components/vc/typography";
import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import { getVariant } from "../../../cached-template-actions";
import ErrorScreen from "@/components/error/error-screen";
import Markdown from 'react-markdown'
import { Separator } from "@/components/ui/separator";

type PageProps = {
    params: { name: string, variant: string };
};


export const dynamic = 'force-dynamic';

export async function generateMetadata({ params: { name, variant } }: { params: any }): Promise<Metadata> {

    const intl = await getIntl();
    const defaultTitle = intl.formatMessage({ id: 'templates' });
    const title = name ? `${defaultTitle} - ${name}:${variant}` : defaultTitle;

    return {
        title
    }
}

export default async function VariantPage({ params: { name, variant } }: PageProps) {

    const { data: template, errors } = await getVariant(name, variant)
    const intl = await getIntl();
    if (errors) {
        return <ErrorScreen errors={errors} />
    }

    return (
        <div className="lg:w-[60%] sm:[95%] md:[75%] mx-auto">
            <Link
                className={buttonVariants({
                    variant: "link",
                    className: "!mx-0 !px-0 mb-7 !-ml-1 ",
                })}
                href="/docs/templates"
            >
                <ArrowLeftIcon className="w-4 h-4 mr-1.5" /> Back to templates
            </Link>
            <div className="flex flex-col gap-3 pb-4 w-full mb-2">
                <h1 className="sm:text-4xl text-3xl font-bold">
                    {template.title}
                </h1>
            </div>
            <div className="!w-full">
                <div className="mb-7">
                    <Typography className="">
                        {template.description}
                    </Typography>
                    <Typography className="mt-2">
                        <Markdown>
                            {template.overviewMarkdown}
                        </Markdown>
                    </Typography>
                    <Typography className="mt-2">
                        <Markdown>
                            {template.specificationMarkdown}
                        </Markdown>
                    </Typography>
                </div>
            </div>
        </div>
    );
}