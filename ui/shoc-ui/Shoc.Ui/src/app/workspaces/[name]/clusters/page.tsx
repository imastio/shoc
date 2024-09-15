import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "../cached-workspace-actions";
import ClusterCardList from "./_components/cluster-card-list";
import WorkspaceClustersClientPage from "./_components/workspace-clusters-client-page";

export const dynamic = 'force-dynamic';

export async function generateMetadata({ params: { name } }: { params: any }): Promise<Metadata> {

  const intl = await getIntl();
  const defaultTitle = intl.formatMessage({ id: 'workspaces.sidebar.clusters' });
  const title = name ? `${defaultTitle} - ${name}` : defaultTitle;

  return {
    title
  }
}

export default async function WorkspaceClustersPage({ params: { name } }: any) {

  const { data: workspace, errors: workspaceErrors } = await getByName(name);
  const intl = await getIntl();

  if (workspaceErrors) {
    return <ErrorScreen errors={workspaceErrors} />
  }

  return <>
    <div className="flex flex-col h-full">
      <WorkspaceClustersClientPage workspaceId={workspace.id} workspaceName={name} />
    </div>
  </>
}
