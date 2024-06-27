import WorkspacesHeader from "./_components/workspaces-header";
import { rpc } from "@/server-actions/rpc";
import ErrorScreen from "@/components/error/error-screen";
import NoWorkspace from "./_components/no-workspace";
import WorkspaceCard from "./_components/workspace-card";
import WorkspaceAddDialogButton from "./_components/workspace-add-dialog-button";
import type { Metadata, ResolvingMetadata } from 'next'
import getIntl from "@/i18n/get-intl";

export const dynamic = 'force-dynamic';

export async function generateMetadata(): Promise<Metadata> {
   
    const intl = await getIntl();

    return {
      title: intl.formatMessage({id: 'workspaces'})
    }
  }

export default async function WorkspacesPage() {

    const { data: items, errors } = await rpc('workspace/user-workspaces/getAll');

    if (errors) {
        return <ErrorScreen errors={errors} />
    }

    return <>
        <div className="flex mx-auto w-full lg:w-3/5 flex-col gap-4 p-4 lg:gap-6 lg:p-6">
            <WorkspacesHeader actions={items.length > 0 ? [<WorkspaceAddDialogButton key="add-workspace" />] : []} />
            { items.length === 0 && <NoWorkspace /> }
            {
                items.length > 0 && items.map((item: any) => <WorkspaceCard key={item.id} workspace={item} />)
            }
        </div>
    </>

}
