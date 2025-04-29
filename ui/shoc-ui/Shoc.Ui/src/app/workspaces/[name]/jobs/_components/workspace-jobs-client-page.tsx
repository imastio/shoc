"use client"

import { useCallback, useEffect, useState } from "react"
import ErrorScreen from "@/components/error/error-screen";
import { rpc } from "@/server-actions/rpc";
import { useIntl } from "react-intl";
import BasicHeader from "@/components/general/basic-header";
import NoJobs from "./no-jobs";
import JobsTable from "./jobs-table";
import { Button } from "@/components/ui/button";
import { ChevronLeft, ChevronRight } from "lucide-react";
import LoadingContainer from "@/components/general/loading-container";
import { JobScope, JobStatus } from "./types";
import ScopeSelector from "./scope-selector";
import StatusSelector from "./status-selector";

const DEFAULT_PAGE_SIZE = 10;

interface FilterOptions {
    scope?: JobScope
    status?: JobStatus
    page: number
    size: number
}

export default function WorkspaceJobsClientPage({ workspaceId, workspaceName }: any) {

    const [progress, setProgress] = useState(true);
    const [data, setData] = useState<any>(null);
    const [errors, setErrors] = useState<any[]>([]);

    const [filter, setFilter] = useState<FilterOptions>({
        scope: undefined,
        page: 0,
        size: DEFAULT_PAGE_SIZE
    });

    const intl = useIntl();

    const load = useCallback(async (workspaceId: string, filter: FilterOptions) => {
        setProgress(true);
        const { data, errors } = await rpc('job/workspace-jobs/getBy', { workspaceId, filter: { ...filter } })

        if (errors) {
            setErrors(errors);
            setData(null);
        } else {
            setErrors([]);
            setData(data)
        }

        setProgress(false);
    }, [])

    useEffect(() => {

        if (!workspaceId) {
            return;
        }

        load(workspaceId, filter)

    }, [workspaceId, load, filter])

    if (errors && errors.length > 0) {
        return <ErrorScreen errors={errors} />
    }

    return <div className="flex flex-col mx-auto w-full h-full">
        <BasicHeader
            title={intl.formatMessage({ id: 'jobs.menu.jobs' })}
            actions={[<div key="workspace-header-operations" className="flex space-x-1">
            </div>]}
        />
            <div className="flex flex-row space-x-2 my-4">
                <ScopeSelector className="w-[150px]" value={filter.scope || 'all'} onChange={newValue => setFilter(prev => ({
                    ...prev,
                    scope: newValue !== 'all' ? newValue as JobScope : undefined,
                    page: 0
                }))} />
                <StatusSelector className="w-[220px]" value={filter.status || 'all'} onChange={newValue => setFilter(prev => ({
                    ...prev,
                    status: newValue !== 'all' ? newValue as JobStatus : undefined,
                    page: 0
                }))} />
            </div>
        {(data?.totalCount === 0) && <NoJobs className="w-full h-min-screen my-4" workspaceId={workspaceId} />}
        <LoadingContainer className="w-full h-min-screen m-auto mt-4" loading={progress}>
            {(!data || data.totalCount > 0) &&
                <div className="flex flex-col">
                    <JobsTable className="mt-4" workspaceName={workspaceName} items={data?.items || []} />
                    {data && data.totalCount > filter.size && <div className="flex mx-auto space-x-2">
                        <Button variant="outline" disabled={filter.page === 0} onClick={() => setFilter((prev: any) => ({
                            ...prev,
                            page: prev.page - 1
                        }))}>
                            <ChevronLeft className="mr-2 w-4 h-4" />
                            {intl.formatMessage({ id: 'global.navigation.prev' })}
                        </Button>
                        <Button variant="outline" disabled={filter.page + 1 >= data?.totalCount / filter.size} onClick={() => setFilter((prev: any) => ({
                            ...prev,
                            page: prev.page + 1
                        }
                        ))}>
                            {intl.formatMessage({ id: 'global.navigation.next' })}
                            <ChevronRight className="ml-2 w-4 h-4" />
                        </Button>
                    </div>
                    }
                </div>
            }
        </LoadingContainer>

    </div>
}