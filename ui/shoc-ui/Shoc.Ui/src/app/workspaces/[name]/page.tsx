import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "./cached-workspace-actions";
import DashboardClientPage from "./_components/dashboard-client-page";

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

  return <DashboardClientPage workspaceId={data.id} workspaceName={data.name} />
}
