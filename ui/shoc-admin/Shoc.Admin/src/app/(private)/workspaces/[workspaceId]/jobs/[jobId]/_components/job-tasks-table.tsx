import { ReloadOutlined, RightOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { localDateTime, localDateTimeWithSec } from "@/extended/format";
import TableContainer from "@/components/general/table-container";
import Link from "next/link";
import { jobTaskTypesMap } from "@/well-known/jobs";
import JobTaskStatus from "@/components/job/job-task-status";
import JobTasksClient from "@/clients/shoc/job/job-tasks-client";

export function JobTasksTable({ workspaceId, jobId, loading, onUpdate = () => { } }: any) {

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [errors, setErrors] = useState<any[]>([]);
    
    const columns = useMemo(() => [
        {
            title: "Sequence",
            key: "sequence",
            dataIndex: "sequence",
        },
        {
            title: "Status",
            key: "status",
            dataIndex: "status",
            render: (status: string) => <JobTaskStatus status={status} />
        },
        {
            title: "Type",
            key: "type",
            dataIndex: "type",
            render: (type: string) => jobTaskTypesMap[type] || type 
        },
        {
            title: "Run By",
            key: "userId",
            dataIndex: "userId",
            ellipsis: true,
            render: (userId: string, row: any) => <Link prefetch={false} href={`/users/${userId}`}>{row.userFullName}</Link>
        },
        {
            title: "Cluster",
            key: "clusterId",
            dataIndex: "clusterId",
            ellipsis: true,
            render: (clusterId: string, row: any) => <Link prefetch={false} href={`/workspaces/${row.workspaceId}/clusters/${clusterId}`}>{row.clusterName}</Link>
        },
        {
            title: "Message",
            key: "message",
            dataIndex: "message",
            ellipsis: true
        },
        {
            title: "Created",
            key: "created",
            dataIndex: "created",
            ellipsis: true,
            width: '200px',
            render: localDateTimeWithSec
        },
        {
            title: "Completed",
            key: "completedAt",
            dataIndex: "completedAt",
            ellipsis: true,
            width: '200px',
            render: localDateTimeWithSec
        },
        {
            title: 'More',
            dataIndex: 'id',
            key: 'id',
            width: '100px',
            render: (id: string, row: any) => <Link prefetch={false} href={`/workspaces/${row.workspaceId}/jobs/${row.jobId}/tasks/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ], []);

    const load = useCallback(async (workspaceId: string, jobId: string) => {
        setProgress(true);
        setErrors([]);

        const result = await withToken((token: string) => selfClient(JobTasksClient).getAllExtended(token, workspaceId, jobId ));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setItems(result.payload)
    }, [withToken])

    useEffect(() => {
        if (!workspaceId) {
            return;
        }
        if (!jobId) {
            return;
        }
        load(workspaceId, jobId);
    }, [load, workspaceId, jobId]);

    return <>
        <ConfigProvider renderEmpty={() => <Empty description={"No tasks to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                    <Button key="refresh" size="middle" disabled={progress || loading} icon={<ReloadOutlined />} onClick={() => load(workspaceId, jobId)} />
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={items}
                    loading={progress}
                    scroll={{ x: true }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
}