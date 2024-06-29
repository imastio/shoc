import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "./cached-workspace-actions";
import { Button } from "@/components/ui/button";
import { ChevronLeftIcon } from "@radix-ui/react-icons";
import { Badge } from "@/components/ui/badge";

export const dynamic = 'force-dynamic';

export async function generateMetadata({ params: { name } }: { params: any }): Promise<Metadata> {

  const intl = await getIntl();
  const defaultTitle = intl.formatMessage({ id: 'workspaces.sidebar.dashboard' });
  const title = name ? `${defaultTitle} - ${name}` : defaultTitle;

  return {
    title
  }
}

export default async function WorkspaceDashboardPage({ params: { name } }: any) {
  const { data, errors } = await getByName(name)
  if (errors) {
    return <ErrorScreen errors={errors} />
  }

  return <>
    <div className="flex mx-auto w-full flex-col gap-4 p-4 lg:gap-6 lg:p-6">
      <div className="flex items-center gap-4">
        <Button variant="outline" size="icon" className="h-7 w-7">
          <ChevronLeftIcon className="h-4 w-4" />
          <span className="sr-only">Back</span>
        </Button>
        <h1 className="flex-1 shrink-0 whitespace-nowrap text-xl font-semibold tracking-tight sm:grow-0">
          Pro Controller
        </h1>
        <Badge variant="outline" className="ml-auto sm:ml-0">
          In stock
        </Badge>
        <div className="hidden items-center gap-2 md:ml-auto md:flex">
          <Button variant="outline" size="sm">
            Discard
          </Button>
          <Button size="sm">Save Product</Button>
        </div>
      </div>
    </div>
  </>

}
