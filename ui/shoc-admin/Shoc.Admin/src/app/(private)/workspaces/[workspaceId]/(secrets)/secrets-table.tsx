import { DeleteOutlined, EditOutlined, KeyOutlined, PlusOutlined, ReloadOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Input, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { localDateTime } from "@/extended/format";
import TableContainer from "@/components/general/table-container";
import SecretsClient from "@/clients/shoc/secret/secrets-client";
import SecretsCreateModal from "./secrets-create-modal";
import SecretsUpdateModal from "./secrets-update-modal";
import SecretsValueUpdateModal from "./secrets-value-update-modal";
import SecretsDeleteModal from "./secrets-delete-modal";

export function SecretsTable({ workspaceId, loading, onUpdate = () => { } }: any) {

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
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

    const load = useCallback(async (id: string) => {
        setProgress(true);
        setErrors([]);

        const result = await withToken((token: string) => selfClient(SecretsClient).getAllExtended(token, id));

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
        load(workspaceId);
    }, [load, workspaceId]);

    return <>
        <SecretsCreateModal
            key="add-modal"
            workspaceId={workspaceId}
            onClose={() => setAddingActive(false)}
            open={addingActive}
            onSuccess={(entity: any) => {
                load(workspaceId);
                onUpdate(entity);
            }}
        />
        <SecretsUpdateModal
            key="edit-modal"
            workspaceId={workspaceId}
            onClose={() => setEditingItem(null)}
            open={editingItem}
            existing={editingItem}
            onSuccess={(entity: any) => {
                load(workspaceId);
                onUpdate(entity);
            }}
        />
        <SecretsValueUpdateModal
            key="edit-value-modal"
            workspaceId={workspaceId}
            onClose={() => setEditingValueItem(null)}
            open={editingValueItem}
            existing={editingValueItem}
            onSuccess={(entity: any) => {
                load(workspaceId);
                onUpdate(entity);
            }}
        />
        <SecretsDeleteModal
            key="delete-modal"
            workspaceId={workspaceId}
            existing={deletingItem}
            onClose={() => setDeletingItem(null)}
            open={deletingItem}
            onSuccess={(entity: any) => {
                load(workspaceId);
                onUpdate(entity);
            }}
        />
        <ConfigProvider renderEmpty={() => <Empty description={"No secrets to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                    <Input key="search-box" placeholder="Type something to search..." disabled={progress || loading} value={search} onChange={(e) => setSearch(e.target.value)} />
                    <Button key="refresh-secrets" size="middle" disabled={progress || loading} icon={<ReloadOutlined />} onClick={() => load(workspaceId)} />
                    <Button key="add-button" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)}>
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