import { ReloadOutlined, RightOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { localDateTime } from "@/extended/format";
import TableContainer from "@/components/general/table-container";
import Link from "next/link";
import { jobScopesMap } from "@/well-known/jobs";
import JobsClient from "@/clients/shoc/job/jobs-client";
import JobStatus from "@/components/job/job-status";

export function JobsTable({ workspaceId, loading, onUpdate = () => { } }: any) {

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [paging, setPaging] = useState({ page: 0, size: 10 });
    const [items, setItems] = useState({ items: [], totalCount: 0 });
    const [errors, setErrors] = useState<any[]>([]);
    
    const columns = useMemo(() => [
        {
            title: "Key",
            key: "localId",
            dataIndex: "localId",
        },
        {
            title: "Status",
            key: "status",
            dataIndex: "status",
            render: (status: string) => <JobStatus status={status} />
        },
        {
            title: "Scope",
            key: "scope",
            dataIndex: "scope",
            render: (scope: string) => jobScopesMap[scope] || scope 
        },
        {
            title: "Run By",
            key: "userId",
            dataIndex: "userId",
            render: (userId: string, row: any) => <Link prefetch={false} href={`/users/${userId}`}>{row.userFullName}</Link>
        },
        {
            title: "Cluster",
            key: "clusterId",
            dataIndex: "clusterId",
            render: (clusterId: string, row: any) => <Link prefetch={false} href={`/workspaces/${row.workspaceId}/clusters/${clusterId}`}>{row.clusterName}</Link>
        },
        {
            title: "Completed",
            key: "completedTasks",
            dataIndex: "completedTasks",
            render: (completedTasks: number, row: any) => `${completedTasks} / ${row.totalTasks}`
        },
        {
            title: "Created",
            key: "created",
            dataIndex: "created",
            ellipsis: true,
            width: '200px',
            render: localDateTime
        },
        {
            title: "Updated",
            key: "updated",
            dataIndex: "updated",
            ellipsis: true,
            width: '200px',
            render: localDateTime
        },
        {
            title: 'More',
            dataIndex: 'id',
            key: 'id',
            width: '100px',
            render: (id: string, row: any) => <Link prefetch={false} href={`/workspaces/${row.workspaceId}/jobs/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ], []);

    const load = useCallback(async (id: string) => {
        setProgress(true);
        setErrors([]);

        const result = await withToken((token: string) => selfClient(JobsClient).getExtendedPage(token, id, {}, paging.page, paging.size ));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setItems(result.payload)
    }, [withToken, paging])

    useEffect(() => {
        if (!workspaceId) {
            return;
        }
        load(workspaceId);
    }, [load, workspaceId]);

    return <>
        <ConfigProvider renderEmpty={() => <Empty description={"No jobs to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                    <Button key="refresh" size="middle" disabled={progress || loading} icon={<ReloadOutlined />} onClick={() => load(workspaceId)} />
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={items.items}
                    loading={progress}
                    scroll={{ x: true }}
                    pagination={{
                        onChange: (page, size) => setPaging({ page: page - 1, size }),
                        showTotal: () => false,
                        total: items.totalCount,
                        hideOnSinglePage: false,
                        showPrevNextJumpers: true,
                        showLessItems: false,
                        defaultPageSize: paging.size,
                        pageSize: paging.size
                    }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
}