"use client"

import { PlusOutlined, RightOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { localDateTime } from "@/extended/format";
import Link from "next/link";
import { selfClient } from "@/clients/shoc";
import TableContainer from "@/components/general/table-container";
import ClusterStatus from "@/components/cluster/cluster-status";
import { clusterTypesMap } from "@/well-known/clusters";
import WorkspaceClustersClient from "@/clients/shoc/cluster/workspace-clusters-client";
import ClustersClient from "@/clients/shoc/cluster/clusters-client";
import ClusterCreateModal from "./cluster-create-modal";

export default function ClustersTable({ workspaceId, loading = false }: any) {
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [items, setItems] = useState<any[]>([]);
    const [errors, setErrors] = useState([]);
    const [creatingActive, setCreatingActive] = useState(false)

    const columns = useMemo(() => [
        {
            title: "Name",
            key: "name",
            dataIndex: "name",
            ellipsis: true
        },
        {
            title: "Status",
            key: "status",
            dataIndex: "status",
            ellipsis: true,
            render: (status: string) => <ClusterStatus status={status} /> 
        },
        {
            title: "Type",
            key: "type",
            dataIndex: "type",
            ellipsis: true,
            render: (type: string) => clusterTypesMap[type] 
        },
        {
            title: "Workspace",
            key: "workspaceId",
            dataIndex: "workspaceId",
            ellipsis: true,
            render: (workspaceId: string, row: any) => workspaceId ? <Link href={`/workspaces/${workspaceId}`}>{row.workspaceName}</Link> : ''
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
            render: (id: string, row: any) => <Link href={`/clusters/${row.workspaceId}/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ], []);

    const load = useCallback(async () => {
        setProgress(true);

        const result = workspaceId ? 
            await withToken((token: string) => selfClient(WorkspaceClustersClient).getAllExtended(token, workspaceId)) :
            await withToken((token: string) => selfClient(ClustersClient).getAllExtended(token));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setItems(result.payload)
    }, [withToken, workspaceId])

    useEffect(() => {
        load();
    }, [load]);

    return <>
        <ClusterCreateModal workspaceId={workspaceId} open={creatingActive} onClose={() => setCreatingActive(false)} onSuccess={() => load()} />
        <ConfigProvider renderEmpty={() => <Empty description={"No clusters to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                    <Button icon={<PlusOutlined />} onClick={() => setCreatingActive(true)}>Create</Button>
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={items}
                    loading={progress || loading}
                    scroll={{ x: true }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
};