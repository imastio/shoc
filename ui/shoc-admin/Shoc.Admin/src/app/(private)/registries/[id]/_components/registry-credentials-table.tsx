import { DeleteOutlined, EditOutlined, KeyOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Space, Table, Tag } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import Link from "next/link";
import { localDateTime } from "@/extended/format";
import TableContainer from "@/components/general/table-container";
import { registryCredentialSourcesMap } from "@/well-known/registries";
import RegistryCredentialsClient from "@/clients/shoc/registry/registry-credentials-client";
import RegistryCredentialsAddModal from "./registry-credentials-add-modal";
import RegistryCredentialsUpdateModal from "./regsitry-credentials-update-modal";
import RegistryCredentialsPasswordUpdateModal from "./regsitry-credentials-password-update-modal";
import RegistryCredentialsDeleteModal from "./registry-credentials-delete-modal";

export function RegistryCredentialsTable({registryId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [errors, setErrors] = useState<any[]>([]);
    const [addingActive, setAddingActive] = useState(false);
    const [editingItem, setEditingItem] = useState(null);
    const [editingPasswordItem, setEditingPasswordItem] = useState(null);
    const [deletingItem, setDeletingItem] = useState(null);

    const columns = useMemo(() => [
        {
            title: "Username",
            key: "username",
            dataIndex: "username",
            ellipsis: true
        },
        {
            title: "Email",
            key: "email",
            dataIndex: "email",
            ellipsis: true
        },
        {
            title: "Source",
            key: "source",
            dataIndex: "source",
            render: (source: string) => registryCredentialSourcesMap[source] 
        },
        {
            title: "Access",
            key: "access",
            dataIndex: 'id',
            ellipsis: true,
            render: (_: any, row: any) => <Space><Tag key="pull-access" color={row.pullAllowed ? 'success': 'error'}>Pull</Tag><Tag key="push-access" color={row.pushAllowed ? 'success': 'error'}>Push</Tag></Space>
        },
        {
            title: "User",
            key: "userFullName",
            dataIndex: "userFullName",
            ellipsis: true,
            render: (userFullName: string, row: any) => row.userId ? <Link href={`/users/${row.userId}`}>{userFullName}</Link> : 'None'
        },
        {
            title: "Workspace",
            key: "workspaceName",
            dataIndex: "workspaceName",
            ellipsis: true,
            render: (workspaceName: string, row: any) => row.workspaceId ? <Link href={`/workspaces/${row.workspaceId}`}>{workspaceName}</Link> : 'None'
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
            title: "Actions",
            key: "actions",
            dataIndex: "id",
            render: (_: string, item: any) => <Space>
                <Button key="edit-button" size="small" icon={<EditOutlined />} onClick={() => setEditingItem(item)} />
                <Button key="edit-password-button" size="small" icon={<KeyOutlined />} onClick={() => setEditingPasswordItem(item)} />
                <Button key="delete-button" size="small" icon={<DeleteOutlined />} danger  onClick={() => setDeletingItem(item)} />
            </Space>
        }
    ], []);

    const load = useCallback(async (id: string) => {
        setProgress(true);
        setErrors([]);

        const result = await withToken((token: string) => selfClient(RegistryCredentialsClient).getAllExtended(token, id));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setItems(result.payload)
    }, [withToken])

    useEffect(() => {
        if (!registryId) {
            return;
        }
        load(registryId);
    }, [load, registryId]);

    return <>
        <RegistryCredentialsAddModal
            key="add-modal"
            registryId={registryId}
            onClose={() => setAddingActive(false)}
            open={addingActive}
            onSuccess={(entity: any) => {
                load(registryId);
                onUpdate(entity);
            }}
        />
        <RegistryCredentialsUpdateModal
            key="edit-modal"
            registryId={registryId}
            onClose={() => setEditingItem(null)}
            open={editingItem}
            existing={editingItem}
            onSuccess={(entity: any) => {
                load(registryId);
                onUpdate(entity);
            }}
        />
        <RegistryCredentialsPasswordUpdateModal
            key="edit-password-modal"
            registryId={registryId}
            onClose={() => setEditingPasswordItem(null)}
            open={editingPasswordItem}
            existing={editingPasswordItem}
            onSuccess={(entity: any) => {
                load(registryId);
                onUpdate(entity);
            }}
        />
        <RegistryCredentialsDeleteModal
            key="delete-modal"
            registryId={registryId}
            existing={deletingItem}
            onClose={() => setDeletingItem(null)}
            open={deletingItem}
            onSuccess={(entity: any) => {
                load(registryId);
                onUpdate(entity);
            }}
        />
        <ConfigProvider renderEmpty={() => <Empty description={"No credentials to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                <Button key="add-button" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)}>
                    Add credential
                </Button>
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