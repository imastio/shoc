import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, Card, ConfigProvider, Descriptions, Empty, List, Skeleton } from "antd";
import { useCallback, useEffect, useState } from "react";
import { UserRoleRecordAddModal } from "./user-role-records-add-modal";
import { UserRoleRecordDeleteModal } from "./user-role-records-delete-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";

export function UserRoleRecordsCard({userId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [addingActive, setAddingActive] = useState(false);
    const [deletingItem, setDeletingItem] = useState(null);

    const load = useCallback(async (id: string) => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UsersClient).getRolesById(token, id));

        setProgress(false);

        if (result.error) {
            return;
        }

        setItems(result.payload)
    }, [withToken])

    useEffect(() => {
        if (!userId) {
            return;
        }
        load(userId);
    }, [load, userId]);

    return <>
        <UserRoleRecordAddModal
            key="add-modal"
            userId={userId}
            onClose={() => setAddingActive(false)}
            open={addingActive}
            onSuccess={(entity: any) => {
                load(userId);
                onUpdate(entity);
            }}
        />
        <UserRoleRecordDeleteModal
            key="delete-modal"
            userId={userId}
            existing={deletingItem}
            onClose={() => setDeletingItem(null)}
            open={deletingItem}
            onSuccess={(entity: any) => {
                load(userId);
                onUpdate(entity);
            }}
        />
        <Card>
            <Skeleton loading={progress} paragraph={{ rows: 5 }}>
                <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="User roles" extra={[
                    <Button key="add-button" type="text" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)} />
                ]} />
                <ConfigProvider theme={{ components: { List: { paddingContentHorizontal: 0 } } }} renderEmpty={() => <Empty description={"The user is not included in any of roles"}></Empty>}>
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
                            <List.Item.Meta title={item.name} />
                        </List.Item>
                        } />
                </ConfigProvider>
            </Skeleton>
        </Card>
    </>
}