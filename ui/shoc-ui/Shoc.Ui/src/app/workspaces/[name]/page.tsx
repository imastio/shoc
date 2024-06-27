import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import { rpc } from "@/server-actions/rpc";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "./cached-actions";

export const dynamic = 'force-dynamic';

export async function generateMetadata(): Promise<Metadata> {
   
    const intl = await getIntl();

    return {
      title: intl.formatMessage({id: 'workspaces'})
    }
  }

export default async function WorkspacesPage({ params: { name } }: any) {
    const { data, errors } = await getByName(name)
    if (errors) {
        return <ErrorScreen errors={errors} />
    }

    return <>
        <div className="flex mx-auto w-full lg:w-3/5 flex-col gap-4 p-4 lg:gap-6 lg:p-6">
            {JSON.stringify(data, null, 4)}
        </div>
    </>

}
