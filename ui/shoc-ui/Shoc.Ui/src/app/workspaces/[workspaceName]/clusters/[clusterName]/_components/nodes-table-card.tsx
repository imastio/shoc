import useClusterConnectivity from "@/providers/cluster-connectivity/use-cluster-connectivity";
import useClusterNodes from "../_providers/cluster-nodes/use-cluster-nodes";
import { useIntl } from "react-intl";
import { ClusterNodeValueType } from "../_providers/cluster-nodes/cluster-nodes-context";
import { useMemo } from "react";
import { ColumnDef, flexRender, getCoreRowModel, useReactTable } from "@tanstack/react-table";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import LoadingContainer from "@/components/general/loading-container";
import { cn } from "@/lib/utils";
import { TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { bytesToSize } from "@/extended/format";

export default function NodesTableCard() {

    const intl = useIntl();
    const { loading: connectivityloading } = useClusterConnectivity();
    const { value: nodes, loading: nodesLoading } = useClusterNodes();

    const progress = connectivityloading || nodesLoading;
    const columns: ColumnDef<ClusterNodeValueType>[] = useMemo(() => [
        {
            accessorKey: "name",
            header: intl.formatMessage({ id: 'global.labels.name' }),
            cell: ({ row }) => row.getValue('name'),
        },
        {
            accessorKey: "usedCpu",
            header: intl.formatMessage({ id: 'clusters.labels.usedCpu' }),
            cell: ({ row }) => <span>
                {row.getValue('usedCpu') ?? 'N/A'}
                {row.original.usedCpu && row.original.allocatableCpu ? ` (${Math.round(row.original.usedCpu * 100 / row.original.allocatableCpu)}%)` : ''}
            </span>,
        },
        {
            accessorKey: "allocatableCpu",
            header: intl.formatMessage({ id: 'clusters.labels.allocatableCpu' }),
            cell: ({ row }) => row.getValue('allocatableCpu'),
        },
        {
            accessorKey: "capacityCpu",
            header: intl.formatMessage({ id: 'clusters.labels.capacityCpu' }),
            cell: ({ row }) => row.getValue('capacityCpu'),
        },
        {
            accessorKey: "usedMemory",
            header: intl.formatMessage({ id: 'clusters.labels.usedMemory' }),
            cell: ({ row }) => <span>
                {bytesToSize(row.getValue('usedMemory')) ?? 'N/A'}
                {row.original.usedMemory && row.original.allocatableMemory ? ` (${Math.round(row.original.usedMemory * 100 / row.original.allocatableMemory)}%)` : ''}
            </span>
        },
        {
            accessorKey: "allocatableMemory",
            header: intl.formatMessage({ id: 'clusters.labels.allocatableMemory' }),
            cell: ({ row }) => bytesToSize(row.getValue('allocatableMemory')),
        },
        {
            accessorKey: "capacityMemory",
            header: intl.formatMessage({ id: 'clusters.labels.capacityMemory' }),
            cell: ({ row }) => bytesToSize(row.getValue('capacityMemory')),
        },
        {
            accessorKey: "allocatableNvidiaGpu",
            header: intl.formatMessage({ id: 'clusters.labels.allocatableNvidiaGpu' }),
            cell: ({ row }) => row.original.allocatableNvidiaGpu,
        },
        {
            accessorKey: "capacityNvidiaGpu",
            header: intl.formatMessage({ id: 'clusters.labels.capacityNvidiaGpu' }),
            cell: ({ row }) => row.original.capacityNvidiaGpu,
        }
    ], [intl])

    const rows = useMemo(() => nodes || [], [nodes]);

    const table = useReactTable({
        data: rows,
        columns,
        getCoreRowModel: getCoreRowModel()
    })

    return (
        <Card>
            <CardHeader>
                <CardTitle className="text-xl">{intl.formatMessage({ id: 'clusters.nodes' })}</CardTitle>
                <CardDescription>{intl.formatMessage({ id: 'clusters.nodes.subtitle' })}</CardDescription>
            </CardHeader>
            <CardContent>
                <LoadingContainer loading={progress}>

                    <div className="rounded-md border">
                        <div className="max-h-[250px] overflow-x-auto">
                            <table
                                data-slot="table"
                                className={cn("w-full caption-bottom text-sm overflow-x-auto")}
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
                                                {row.getAllCells().map((cell) => (
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
                                                {progress && intl.formatMessage({ id: 'global.notice.loading' })}
                                                {!progress && intl.formatMessage({ id: 'clusters.nodes.noNodes' })}
                                            </TableCell>
                                        </TableRow>
                                    )}
                                </TableBody>
                            </table>

                        </div>
                    </div>
                </LoadingContainer>
            </CardContent>
        </Card>
    )
}