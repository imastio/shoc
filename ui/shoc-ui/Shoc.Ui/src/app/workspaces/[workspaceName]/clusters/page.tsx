import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "../cached-workspace-actions";
import WorkspaceClustersClientPage from "./_components/workspace-clusters-client-page";
import WorkspacePageWrapper from "../_components/workspace-page-wrapper";
import WorkspacePageHeader from "@/components/general/workspace-page-header";

export const dynamic = 'force-dynamic';

export async function generateMetadata(props: { params: Promise<any> }): Promise<Metadata> {
  const params = await props.params;

  const {
    workspaceName
  } = params;

  const intl = await getIntl();
  const defaultTitle = intl.formatMessage({ id: 'workspaces.sidebar.clusters' });
  const title = workspaceName ? `${defaultTitle} - ${workspaceName}` : defaultTitle;

  return {
    title
  }
}

export default async function WorkspaceClustersPage(props: any) {
  const params = await props.params;

  const {
    workspaceName
  } = params;

  const { data: workspace, errors: workspaceErrors } = await getByName(workspaceName);
  const intl = await getIntl();

  if (workspaceErrors) {
    return <ErrorScreen errors={workspaceErrors} />
  }

  return <WorkspacePageWrapper header={
    <WorkspacePageHeader title={intl.formatMessage({ id: 'workspaces.clusters.page.title' })} />
  }>
    <WorkspaceClustersClientPage workspaceId={workspace.id} workspaceName={workspaceName} />
  </WorkspacePageWrapper>
}
