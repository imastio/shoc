import LoadingContainer from "@/components/general/loading-container";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { rpc } from "@/server-actions/rpc";
import Link from "next/link";
import { useCallback, useEffect, useState } from "react";
import { useIntl } from "react-intl";

export default function WorkspaceMembersCountCard({ workspaceId, workspaceName }: { workspaceId: string, workspaceName: string }) {

    const intl = useIntl();
    const [progress, setProgress] = useState(true);
    const [data, setData] = useState<any>(null);

    const load = useCallback(async (workspaceId: string) => {

        setProgress(true);
        const { data, errors } = await rpc('cluster/workspace-clusters/countAll', { workspaceId })

        if (errors) {
            setData(null);
        } else {
            setData(data)
        }

        setProgress(false);

    }, []);

    useEffect(() => {
        if (!workspaceId) {
            return;
        }
        load(workspaceId);
    }, [workspaceId, load])

    return <LoadingContainer loading={progress}>
        <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium">
                    {intl.formatMessage({ id: 'workspaces.members.card.count' })}
                </CardTitle>
            </CardHeader>
            <CardContent>
                <div className="text-2xl font-bold">{data?.count || "N/A"}</div>
            </CardContent>
            <CardFooter>
                <Link href={`/workspaces/${workspaceName}/members`}>
                    <Button variant="link" className="px-0">
                        {intl.formatMessage({id: 'global.actions.more'})}
                    </Button>
                </Link>
            </CardFooter>
        </Card>
    </LoadingContainer>
}