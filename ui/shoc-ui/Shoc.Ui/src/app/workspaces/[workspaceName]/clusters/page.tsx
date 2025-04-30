import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "../cached-workspace-actions";
import WorkspaceClustersClientPage from "./_components/workspace-clusters-client-page";

export const dynamic = 'force-dynamic';

export async function generateMetadata({ params: { workspaceName } }: { params: any }): Promise<Metadata> {

  const intl = await getIntl();
  const defaultTitle = intl.formatMessage({ id: 'workspaces.sidebar.clusters' });
  const title = workspaceName ? `${defaultTitle} - ${workspaceName}` : defaultTitle;

  return {
    title
  }
}

export default async function WorkspaceClustersPage({ params: { workspaceName } }: any) {

  const { data: workspace, errors: workspaceErrors } = await getByName(workspaceName);

  if (workspaceErrors) {
    return <ErrorScreen errors={workspaceErrors} />
  }

  return <>
    <div className="flex flex-col h-full">
      <WorkspaceClustersClientPage workspaceId={workspace.id} workspaceName={workspaceName} />
    </div>
  </>
}
