import getIntl from "@/i18n/get-intl";
import { Metadata } from "next";
import ErrorScreen from "@/components/error/error-screen";
import { getByName } from "../cached-workspace-actions";
import WorkspaceMembersTable from "./_components/workspace-members-table";

export const dynamic = 'force-dynamic';

export async function generateMetadata({ params: { workspaceName } }: { params: any }): Promise<Metadata> {

  const intl = await getIntl();
  const defaultTitle = intl.formatMessage({ id: 'workspaces.sidebar.members' });
  const title = workspaceName ? `${defaultTitle} - ${workspaceName}` : defaultTitle;

  return {
    title
  }
}

export default async function WorkspaceMembersPage({ params: { workspaceName } }: any) {

  const { data: workspace, errors: workspaceErrors } = await getByName(workspaceName);
  const intl = await getIntl();

  if (workspaceErrors) {
    return <ErrorScreen errors={workspaceErrors} />
  }

  return <>
    <div className="items-center">
      <h1 className="text-lg truncate font-semibold md:text-2xl">{intl.formatMessage({id: 'workspaces.sidebar.members'})}</h1>
    </div>
    <WorkspaceMembersTable className="mt-4" workspaceId={workspace.id} />
  </>
}
