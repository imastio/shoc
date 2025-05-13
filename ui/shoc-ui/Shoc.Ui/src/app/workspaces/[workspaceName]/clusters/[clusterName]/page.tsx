import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "../../cached-workspace-actions";
import { getClusterByName } from "../cached-cluster-actions";
import WorkspacePageWrapper from "../../_components/workspace-page-wrapper";
import WorkspacePageHeader from "@/components/general/workspace-page-header";
import WorkspacePageBreadcrumbs from "@/components/general/workspace-page-breadcrumbs";
import { BreadcrumbLink } from "@/components/ui/breadcrumb";
import { Button } from "@/components/ui/button";
import { ReloadIcon } from "@radix-ui/react-icons";

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
    workspaceName,
    clusterName
  } = params;

  const intl = await getIntl();
  const { data: workspace, errors: workspaceErrors } = await getByName(workspaceName);

  if (workspaceErrors) {
    return <ErrorScreen errors={workspaceErrors} />
  }

  const { data: cluster, errors: clusterErrors } = await getClusterByName(workspace.id, clusterName);

  if (clusterErrors) {
    return <ErrorScreen errors={clusterErrors} />
  }

  return <WorkspacePageWrapper header={
    <WorkspacePageHeader breadcrumb={
      <WorkspacePageBreadcrumbs crumbs={[
        <BreadcrumbLink key="clusters" href={`/workspaces/${cluster.workspaceName}/clusters`}>{intl.formatMessage({ id: 'workspaces.sidebar.clusters' })}</BreadcrumbLink>
      ]}
        title={cluster.name} />
    } actions={
      <Button size="sm" variant="outline"><ReloadIcon className="w-4 h-4" /></Button>
    }
    />
  }>
    <pre>
      {JSON.stringify(cluster, null, 4)}
    </pre>
  </WorkspacePageWrapper>
}
