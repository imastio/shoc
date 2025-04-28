"use client"

import { useCallback, useEffect, useState } from "react"
import ErrorScreen from "@/components/error/error-screen";
import { rpc } from "@/server-actions/rpc";
import { useIntl } from "react-intl";
import BasicHeader from "@/components/general/basic-header";
import NoJobs from "./no-jobs";
import JobsTable from "./jobs-table";

const DEFAULT_SIZE = 10;

export default function WorkspaceJobsClientPage({ workspaceId, workspaceName }: any) {

    const [progress, setProgress] = useState(true);
    const [data, setData] = useState<any>(null);
    const [errors, setErrors] = useState<any[]>([]);
    const [paging, setPaging] = useState<{ page: number, size: number }>({ page: 0, size: DEFAULT_SIZE })
    const intl = useIntl();

    const load = useCallback(async (workspaceId: string, page: number, size: number) => {
        setProgress(true);
        const { data, errors } = await rpc('job/workspace-jobs/getBy', { workspaceId, filter: { page, size } })

        if (errors) {
            setErrors(errors);
            setData(null);
        } else {
            console.log("data", data)
            setErrors([]);
            setData(data)
        }

        setProgress(false);
    }, [])

    useEffect(() => {

        if (!workspaceId) {
            return;
        }

        load(workspaceId, paging.page, paging.size)

    }, [workspaceId, paging])

    if (errors && errors.length > 0) {
        return <ErrorScreen errors={errors} />
    }

    return <div className="flex flex-col mx-auto w-full h-full">
        <BasicHeader
            title={intl.formatMessage({ id: 'jobs.menu.jobs' })}
            actions={[<div key="workspace-header-operations" className="flex space-x-1">
            </div>]}
        />

        <JobsTable className="mt-4" workspaceName={workspaceName} items={data?.items || []} />
    </div>
}