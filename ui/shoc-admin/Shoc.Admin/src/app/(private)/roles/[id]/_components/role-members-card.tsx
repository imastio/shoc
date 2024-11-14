import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, Card, ConfigProvider, Descriptions, Empty, List, Skeleton } from "antd";
import { useCallback, useEffect, useState } from "react";
import { RoleMembersAddModal } from "./role-members-add-modal";
import { RoleMembersDeleteModal } from "./role-members-delete-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import RoleMembersClient from "@/clients/shoc/identity/role-members-client";

export function RoleMembersCard({roleId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [addingActive, setAddingActive] = useState(false);
    const [deletingItem, setDeletingItem] = useState(null);

    const load = useCallback(async (id: string) => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(RoleMembersClient).getAll(token, id));

        setProgress(false);

        if (result.error) {
            return;
        }

        setItems(result.payload)
    }, [withToken])

    useEffect(() => {
        if (!roleId) {
            return;
        }
        load(roleId);
    }, [load, roleId]);

    return <>
        <RoleMembersAddModal
            key="add-modal"
            roleId={roleId}
            onClose={() => setAddingActive(false)}
            open={addingActive}
            onSuccess={(entity: any) => {
                load(roleId);
                onUpdate(entity);
            }}
        />
        <RoleMembersDeleteModal
            key="delete-modal"
            roleId={roleId}
            existing={deletingItem}
            onClose={() => setDeletingItem(null)}
            open={deletingItem}
            onSuccess={(entity: any) => {
                load(roleId);
                onUpdate(entity);
            }}
        />
        <Card>
            <Skeleton loading={progress} paragraph={{ rows: 5 }}>
                <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="Role members" extra={[
                    <Button key="add-button" type="text" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)} />
                ]} />
                <ConfigProvider theme={{ components: { List: { paddingContentHorizontal: 0 } } }} renderEmpty={() => <Empty description={"There are not users in this role"}></Empty>}>
                    <List
                        split={false}
                        size="small"
                        bordered={false}
                        loading={progress}
                        dataSource={items}
                        rowKey={item => item.id}
                        renderItem={item => <List.Item key={item.id}
                            actions={[
                                <Button key="delete-button" size="small" icon={<DeleteOutlined />} danger onClick={() => setDeletingItem(item)} />
                            ]}>
                            <List.Item.Meta title={item.fullName} description={item.email} />
                        </List.Item>
                        } />
                </ConfigProvider>
            </Skeleton>
        </Card>
    </>
}