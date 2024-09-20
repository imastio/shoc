import { DeleteOutlined, EditOutlined, KeyOutlined, PlusOutlined, ReloadOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Input, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { localDateTime } from "@/extended/format";
import TableContainer from "@/components/general/table-container";
import UserSecretsClient from "@/clients/shoc/secret/user-secrets-client";
import WorkspaceUserSecretsDeleteModal from "./workspace-user-secrets-delete-modal";
import WorkspaceUserSecretsValueUpdateModal from "./workspace-user-secrets-value-update-modal";
import WorkspaceUserSecretsUpdateModal from "./workspace-user-secrets-update-modal";
import WorkspaceUserSecretsCreateModal from "./workspace-user-secrets-create-modal";
import { WorkspaceMemberSelector } from "@/components/workspace/workspace-member-selector";

export function WorkspaceUserSecretsTable({ workspaceId, loading, onUpdate = () => { } }: any) {

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [userId, setUserId] = useState(null);
    const [items, setItems] = useState<any[]>([]);
    const [errors, setErrors] = useState<any[]>([]);
    const [search, setSearch] = useState<string>('');
    const [addingActive, setAddingActive] = useState(false);
    const [editingItem, setEditingItem] = useState(null);
    const [editingValueItem, setEditingValueItem] = useState(null);
    const [deletingItem, setDeletingItem] = useState(null);

    const filteredItems = useMemo(() => {
        return items.filter(item => (item.name as string ?? '').toLowerCase().includes(search.toLowerCase()))
    }, [items, search])

    const columns = useMemo(() => [
        {
            title: "Name",
            key: "name",
            dataIndex: "name",
            ellipsis: true
        },
        {
            title: "Description",
            key: "description",
            dataIndex: "description",
            ellipsis: true
        },
        {
            title: "Disabled",
            key: "disabled",
            dataIndex: "disabled",
            render: (disabled: boolean) => disabled ? 'Disabled' : 'Enabled'
        },
        {
            title: "Encrypted",
            key: "encrypted",
            dataIndex: "encrypted",
            render: (encrypted: boolean) => encrypted ? 'Encrypted' : 'Plain'
        },
        {
            title: "Value",
            key: "value",
            dataIndex: "value",
            ellipsis: true,
            render: (value: string, row: any) => row.encrypted ? '**********' : value
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
                <Button key="edit-value-button" size="small" icon={<KeyOutlined />} onClick={() => setEditingValueItem(item)} />
                <Button key="delete-button" size="small" icon={<DeleteOutlined />} danger onClick={() => setDeletingItem(item)} />
            </Space>
        }
    ], []);

    const load = useCallback(async (workspaceId: string, userId: string | null) => {
        
        if(!userId){
            setItems([]);
            setErrors([]);
            setProgress(false);
            return;
        }

        setProgress(true);
        setErrors([]);

        const result = await withToken((token: string) => selfClient(UserSecretsClient).getAllExtended(token, workspaceId, userId));

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
        load(workspaceId, userId);
    }, [load, workspaceId, userId]);

    return <>
        <WorkspaceUserSecretsCreateModal
            key="add-modal"
            workspaceId={workspaceId}
            userId={userId}
            onClose={() => setAddingActive(false)}
            open={addingActive}
            onSuccess={(entity: any) => {
                load(workspaceId, userId);
                onUpdate(entity);
            }}
        />
        <WorkspaceUserSecretsUpdateModal
            key="edit-modal"
            workspaceId={workspaceId}
            userId={userId}
            onClose={() => setEditingItem(null)}
            open={editingItem}
            existing={editingItem}
            onSuccess={(entity: any) => {
                load(workspaceId, userId);
                onUpdate(entity);
            }}
        />
        <WorkspaceUserSecretsValueUpdateModal
            key="edit-value-modal"
            workspaceId={workspaceId}
            userId={userId}
            onClose={() => setEditingValueItem(null)}
            open={editingValueItem}
            existing={editingValueItem}
            onSuccess={(entity: any) => {
                load(workspaceId, userId);
                onUpdate(entity);
            }}
        />
        <WorkspaceUserSecretsDeleteModal
            key="delete-modal"
            workspaceId={workspaceId}
            userId={userId}
            existing={deletingItem}
            onClose={() => setDeletingItem(null)}
            open={deletingItem}
            onSuccess={(entity: any) => {
                load(workspaceId, userId);
                onUpdate(entity);
            }}
        />
        <ConfigProvider renderEmpty={() => <Empty description={"No secrets to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                    <WorkspaceMemberSelector key="user-selector" placeholder="Select a user" workspaceId={workspaceId} value={userId} disabled={progress || loading} style={{width: '150px'}} onChange={setUserId} />
                    <Input key="search-box" placeholder="Type something to search..." disabled={progress || loading || !userId} value={search} onChange={(e) => setSearch(e.target.value)} />
                    <Button key="refresh-secrets" size="middle" disabled={progress || loading || !userId} icon={<ReloadOutlined />} onClick={() => load(workspaceId, userId)} />
                    <Button key="add-button" size="middle" disabled={progress || loading || !userId} icon={<PlusOutlined />} onClick={() => setAddingActive(true)}>
                        New secret
                    </Button>
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={filteredItems}
                    loading={progress}
                    scroll={{ x: true }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
}