import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "./cached-workspace-actions";
import DashboardClientPage from "./_components/dashboard-client-page";
import WorkspacePageHeader from "@/components/general/workspace-page-header";
import WorkspacePageWrapper from "./_components/workspace-page-wrapper";

export const dynamic = 'force-dynamic';

export async function generateMetadata(props: { params: Promise<any> }): Promise<Metadata> {
  const params = await props.params;

  const {
    workspaceName
  } = params;

  const intl = await getIntl();
  const defaultTitle = intl.formatMessage({ id: 'workspaces.sidebar.dashboard' });
  const title = workspaceName ? `${defaultTitle} - ${workspaceName}` : defaultTitle;

  return {
    title
  }
}

export default async function WorkspaceDashboardPage(props: any) {
  const params = await props.params;

  const {
    workspaceName
  } = params;
  const intl = await getIntl();

  const { errors } = await getByName(workspaceName)

  if (errors) {
    return <ErrorScreen errors={errors} />
  }

  return <WorkspacePageWrapper header={
    <WorkspacePageHeader title={intl.formatMessage({ id: 'workspaces.sidebar.dashboard' })} />
    }>
        <DashboardClientPage />
    </WorkspacePageWrapper>
    
}
