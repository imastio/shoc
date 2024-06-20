import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, Card, ConfigProvider, Descriptions, Empty, List, Skeleton } from "antd";
import { useCallback, useEffect, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import WorkspaceMembersClient from "@/clients/shoc/workspace/workspace-members-client";
import WorkspaceMembersAddModal from "./workspace-members-add-modal";
import WorkspaceMembersDeleteModal from "./workspace-members-delete-modal";
import WorkspaceMembersUpdateModal from "./workspace-members-update-modal";
import { workspaceRolesMap } from "@/well-known/workspaces";

export function WorkspaceMembersCard({workspaceId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [addingActive, setAddingActive] = useState(false);
    const [editingItem, setEditingItem] = useState(null);
    const [deletingItem, setDeletingItem] = useState(null);

    const load = useCallback(async (id: string) => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(WorkspaceMembersClient).getAllExtended(token, id));

        setProgress(false);

        if (result.error) {
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
        <Card>
            <Skeleton loading={progress} paragraph={{ rows: 5 }}>
                <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="Workspace members" extra={[
                    <Button key="add-button" type="text" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)} />
                ]} />
                <ConfigProvider theme={{ components: { List: { paddingContentHorizontal: 0 } } }} renderEmpty={() => <Empty description={"There are not members in this workspace"}></Empty>}>
                    <List
                        split={false}
                        size="small"
                        bordered={false}
                        loading={progress}
                        dataSource={items}
                        rowKey={item => item.id}
                        renderItem={item => <List.Item key={item.id}
                            actions={[
                                <Button key="edit-button" size="small" icon={<EditOutlined />} onClick={() => setEditingItem(item)} />,
                                <Button key="delete-button" size="small" icon={<DeleteOutlined />} danger onClick={() => setDeletingItem(item)} />
                            ]}>
                            <List.Item.Meta title={`${item.fullName} (${workspaceRolesMap[item.role] || item.role})`} description={item.email} />
                        </List.Item>
                        } />
                </ConfigProvider>
            </Skeleton>
        </Card>
    </>
}