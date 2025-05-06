"use client"

import {
    ColumnDef,
    flexRender,
    getCoreRowModel,
    getFilteredRowModel,
    getPaginationRowModel,
    getSortedRowModel,
    useReactTable,
} from "@tanstack/react-table"

import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"
import { useIntl } from "react-intl"
import { FilterOptions, Job } from "../jobs/_components/types"
import JobStatusBadge from "../jobs/_components/job-status-badge"
import { useSession } from "next-auth/react"
import { useCallback, useEffect, useMemo, useState } from "react"
import { rpc } from "@/server-actions/rpc"
import useWorkspace from "@/providers/workspace/use-workspace"
import { User } from "@auth/core/types"
import { cn } from "@/lib/utils"
import Link from "next/link"
import { durationBetween } from "@/extended/format"


export default function DashboardJobsCard() {
    const intl = useIntl();
    const session = useSession();
    const [progress, setProgress] = useState<boolean>(true);
    const [errors, setErrors] = useState<any[]>([]);
    const [data, setData] = useState<any>(null);
    const { id: workspaceId, name: workspaceName } = useWorkspace();
    const userId = (session?.data?.user as User)?.userId;

    const filter = useMemo<FilterOptions>(() => ({
        scope: undefined,
        status: undefined,
        userId: userId,
        page: 0,
        size: 10
    }), [userId]);

    const columns: ColumnDef<Job>[] = useMemo(() => [
        {
            accessorKey: "localId",
            header: "#",
            cell: ({ row }) => (
                <div className="capitalize">{row.getValue("localId")}</div>
            ),
        },
        {
            accessorKey: "name",
            header: intl.formatMessage({ id: 'global.labels.name' }),
            cell: ({ row }) => <Link prefetch={false} className="underline" href={`/workspaces/${workspaceName}/jobs/${row.original.localId}`}>
                {row.getValue('name')}
            </Link>
        },
        {
            accessorKey: "status",
            header: intl.formatMessage({ id: 'global.labels.status' }),
            cell: ({ row }) => <JobStatusBadge status={row.getValue("status")} />,
        },
        {
            accessorKey: "clusterId",
            header: intl.formatMessage({ id: 'jobs.labels.cluster' }),
            cell: ({ row }) => <Link prefetch={false} className="underline" href={`/workspaces/${workspaceName}/clusters/${row.original.clusterName}`}>
                {row.original.clusterName}
            </Link>,
        },
        {
            accessorKey: "waiting",
            header: intl.formatMessage({ id: 'jobs.labels.waiting' }),
            cell: ({ row }) => durationBetween(row.original.created, row.original.runningAt) ,
        },
        {
            accessorKey: "running",
            header: intl.formatMessage({ id: 'jobs.labels.running' }),
            cell: ({ row }) => row.original.runningAt ? durationBetween(row.original.runningAt, row.original.completedAt) : "N/A",
        },
    ], [intl, workspaceName])


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

        if (!workspaceId || !filter.userId) {
            return;
        }

        load(workspaceId, filter)

    }, [workspaceId, load, filter])

    const table = useReactTable({
        data: data?.items || [],
        columns,
        getCoreRowModel: getCoreRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
        getSortedRowModel: getSortedRowModel(),
        getFilteredRowModel: getFilteredRowModel(),
    })

    return (
        <Card>
            <CardHeader>
                <CardTitle className="text-xl">{intl.formatMessage({ id: 'jobs' })}</CardTitle>
                <CardDescription>{intl.formatMessage({ id: 'jobs.dashboard.subtitle' })}</CardDescription>
            </CardHeader>
            <CardContent>
                <div className="rounded-md border">
                    <div className="max-h-[250px] overflow-x-auto">
                        <table
                            data-slot="table"
                            className={cn("w-full caption-bottom text-sm")}
                        >
                            <TableHeader>
                                {table.getHeaderGroups().map((headerGroup) => (
                                    <TableRow key={headerGroup.id}>
                                        {headerGroup.headers.map((header) => {
                                            return (
                                                <TableHead
                                                    key={header.id}
                                                    className="[&:has([role=checkbox])]:pl-3 bg-white sticky top-0"
                                                >
                                                    {header.isPlaceholder
                                                        ? null
                                                        : flexRender(
                                                            header.column.columnDef.header,
                                                            header.getContext()
                                                        )}
                                                </TableHead>
                                            )
                                        })}
                                    </TableRow>
                                ))}
                            </TableHeader>
                            <TableBody>
                                {table.getRowModel().rows?.length ? (
                                    table.getRowModel().rows.map((row) => (
                                        <TableRow
                                            key={row.id}
                                            data-state={row.getIsSelected() && "selected"}
                                        >
                                            {row.getVisibleCells().map((cell) => (
                                                <TableCell
                                                    key={cell.id}
                                                    className="[&:has([role=checkbox])]:pl-3"
                                                >
                                                    {flexRender(
                                                        cell.column.columnDef.cell,
                                                        cell.getContext()
                                                    )}
                                                </TableCell>
                                            ))}
                                        </TableRow>
                                    ))
                                ) : (
                                    <TableRow>
                                        <TableCell
                                            colSpan={columns.length}
                                            className="h-24 text-center"
                                        >
                                            {intl.formatMessage({ id: 'jobs.dashboard.noJobs' })}
                                        </TableCell>
                                    </TableRow>
                                )}
                            </TableBody>
                        </table>

                    </div>
                </div>
                <div className="flex items-center justify-end space-x-2 pt-4">
                    <div className="flex-1 text-sm text-muted-foreground">
                        {intl.formatMessage(
                            { id: 'jobs.dashboard.footnote' }, 
                            {
                            count: table.getFilteredRowModel().rows.length,
                            total: data?.totalCount ?? 0
                            }
                        )}
                        {' '}
                        <Link prefetch={false} className="underline" href={`/workspaces/${workspaceName}/jobs`}>
                            {intl.formatMessage({id: 'global.actions.seeAll'})}
                        </Link>
                    </div>
                    
                </div>
            </CardContent>
        </Card>
    )
}
