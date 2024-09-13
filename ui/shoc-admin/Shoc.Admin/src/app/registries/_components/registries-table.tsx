"use client"

import { PlusOutlined, RightOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { localDateTime } from "@/extended/format";
import Link from "next/link";
import { selfClient } from "@/clients/shoc";
import TableContainer from "@/components/general/table-container";
import { registryProviderTypesMap, registryUsageScopesMap } from "@/well-known/registries";
import RegistriesClient from "@/clients/shoc/registry/registries-client";
import RegistryCreateModal from "./registry-create-modal";
import RegistryStatus from "@/components/registry/registry-status";

export default function RegistriesTable() {
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
            title: "Display Name",
            key: "displayName",
            dataIndex: "displayName",
            ellipsis: true
        },
        {
            title: "Registry",
            key: "registry",
            dataIndex: "registry",
            ellipsis: true
        },
        {
            title: "Namespace",
            key: "namespace",
            dataIndex: "namespace",
            ellipsis: true
        },
        {
            title: "Status",
            key: "status",
            dataIndex: "status",
            ellipsis: true,
            render: (status: string) => <RegistryStatus status={status} /> 
        },
        {
            title: "Provider",
            key: "provider",
            dataIndex: "provider",
            ellipsis: true,
            render: (provider: string) => registryProviderTypesMap[provider] 
        },
        {
            title: "Usage Scope",
            key: "usageScope",
            dataIndex: "usageScope",
            ellipsis: true,
            render: (usageScope: string) => registryUsageScopesMap[usageScope] 
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
            render: (id: string) => <Link href={`/registries/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ], []);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(RegistriesClient).getAllExtended(token));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setItems(result.payload)
    }, [withToken])

    useEffect(() => {
        load();
    }, [load]);

    return <>
        <RegistryCreateModal open={creatingActive} onClose={() => setCreatingActive(false)} onSuccess={() => load()} />
        <ConfigProvider renderEmpty={() => <Empty description={"No registries to display"}></Empty>}>
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
                    loading={progress}
                    scroll={{ x: true }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
};