"use client"

import { PlusOutlined, RightOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import WorkspaceCreateModal from "./workspace-create-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { localDateTime } from "@/extended/format";
import Link from "next/link";
import { selfClient } from "@/clients/shoc";
import TableContainer from "@/components/general/table-container";
import WorkspacesClient from "@/clients/shoc/workspace/workspaces-client";
import { workspaceStatusesMap, workspaceTypesMap } from "@/well-known/workspaces";

export default function WorkspacesTable() {
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [roles, setRoles] = useState<any[]>([]);
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
            title: "Title",
            key: "title",
            dataIndex: "title",
            ellipsis: true
        },
        {
            title: "Type",
            key: "type",
            dataIndex: "type",
            render: (type: string) => workspaceTypesMap[type] 
        },
        {
            title: "Status",
            key: "status",
            dataIndex: "status",
            render: (status: string) => workspaceStatusesMap[status] 
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
            render: (id: string) => <Link href={`/workspaces/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ], []);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(WorkspacesClient).getAll(token));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setRoles(result.payload)
    }, [withToken])

    useEffect(() => {
        load();
    }, [load]);

    return <>
        <WorkspaceCreateModal open={creatingActive} onClose={() => setCreatingActive(false)} onSuccess={(entity: any) => setRoles([...roles, entity])} />
        <ConfigProvider renderEmpty={() => <Empty description={"No workspaces to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                    <Button icon={<PlusOutlined />} onClick={() => setCreatingActive(true)}>Create</Button>
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={roles}
                    loading={progress}
                    scroll={{ x: true }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
};