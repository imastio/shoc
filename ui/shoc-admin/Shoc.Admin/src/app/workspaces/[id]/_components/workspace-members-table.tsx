import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import WorkspaceMembersAddModal from "./workspace-members-add-modal";
import WorkspaceMembersDeleteModal from "./workspace-members-delete-modal";
import WorkspaceMembersUpdateModal from "./workspace-members-update-modal";
import { workspaceRolesMap } from "@/well-known/workspaces";
import Link from "next/link";
import { localDateTime } from "@/extended/format";
import TableContainer from "@/components/general/table-container";
import WorkspaceMembersClient from "@/clients/shoc/workspace/workspace-members-client";

export function WorkspaceMembersTable({workspaceId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [errors, setErrors] = useState<any[]>([]);
    const [addingActive, setAddingActive] = useState(false);
    const [editingItem, setEditingItem] = useState(null);
    const [deletingItem, setDeletingItem] = useState(null);

    const columns = useMemo(() => [
        {
            title: "Full Name",
            key: "fullName",
            dataIndex: "fullName",
            ellipsis: true,
            render: (fullName: string, row: any) => <Link href={`/users/${row.userId}`}>{fullName}</Link>
        },
        {
            title: "Email",
            key: "email",
            dataIndex: "email",
            ellipsis: true
        },
        {
            title: "Role",
            key: "role",
            dataIndex: "role",
            render: (role: string) => workspaceRolesMap[role] 
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
                <Button key="edit-button" size="small" icon={<EditOutlined />} disabled={item.role === 'owner'} onClick={() => setEditingItem(item)} />
                <Button key="delete-button" size="small" icon={<DeleteOutlined />} danger disabled={item.role === 'owner'} onClick={() => setDeletingItem(item)} />
            </Space>
        }
    ], []);

    const load = useCallback(async (id: string) => {
        setProgress(true);
        setErrors([]);

        const result = await withToken((token: string) => selfClient(WorkspaceMembersClient).getAllExtended(token, id));

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
        <WorkspaceMembersAddModal
            key="add-modal"
            workspaceId={workspaceId}
            onClose={() => setAddingActive(false)}
            open={addingActive}
            onSuccess={(entity: any) => {
                load(workspaceId);
                onUpdate(entity);
            }}
        />
        <WorkspaceMembersUpdateModal
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
        <WorkspaceMembersDeleteModal
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
        <ConfigProvider renderEmpty={() => <Empty description={"No members to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                <Button key="add-button" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)}>
                    Add member
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