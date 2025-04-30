import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "../../cached-workspace-actions";
import { getClusterByName } from "../cached-cluster-actions";

export const dynamic = 'force-dynamic';

export async function generateMetadata({ params: { workspaceName } }: { params: any }): Promise<Metadata> {

  const intl = await getIntl();
  const defaultTitle = intl.formatMessage({ id: 'workspaces.sidebar.clusters' });
  const title = workspaceName ? `${defaultTitle} - ${workspaceName}` : defaultTitle;

  return {
    title
  }
}

export default async function WorkspaceClustersPage({ params: { workspaceName, clusterName } }: any) {

  const intl = await getIntl();
  const { data: workspace, errors: workspaceErrors } = await getByName(workspaceName);

  if (workspaceErrors) {
    return <ErrorScreen errors={workspaceErrors} />
  }

  const { data: cluster, errors: clusterErrors } = await getClusterByName(workspace.id, clusterName);

  if (clusterErrors) {
    return <ErrorScreen errors={clusterErrors} />
  }

  return <>
    <div className="flex flex-col h-full">
      {JSON.stringify(cluster, null, 4)}
    </div>
  </>
}
