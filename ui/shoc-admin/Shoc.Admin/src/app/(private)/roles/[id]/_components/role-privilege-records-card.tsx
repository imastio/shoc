import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, Card, ConfigProvider, Descriptions, Empty, List, Skeleton } from "antd";
import { useCallback, useEffect, useState } from "react";
import { RolePrivilegeRecordsAddModal } from "./role-privilege-records-add-modal";
import { RolePrivilegeRecordsDeleteModal } from "./role-privilege-records-delete-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import RolePrivilegesClient from "@/clients/shoc/identity/role-privileges-client";

export function RolePrivilegeRecordsCard({roleId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [rolePrivileges, setRolePrivileges] = useState<any[]>([]);
    const [addingActive, setAddingActive] = useState(false);
    const [deletingItem, setDeletingItem] = useState(null);

    const load = useCallback(async (id: string) => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(RolePrivilegesClient).getAll(token, id));

        setProgress(false);

        if (result.error) {
            return;
        }

        setRolePrivileges(result.payload)
    }, [withToken])

    useEffect(() => {
        if (!roleId) {
            return;
        }
        load(roleId);
    }, [load, roleId]);

    return <>   
        <RolePrivilegeRecordsAddModal 
            key="add-modal"
            roleId={roleId}
            onClose={() => setAddingActive(false)}
            granted={rolePrivileges}
            open={addingActive}
            onSuccess={() => {
                load(roleId);
                onUpdate();
            }}
        />
        <RolePrivilegeRecordsDeleteModal 
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
                <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="Role Privileges" extra={[
                    <Button key="add-button" type="text" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)} />
                ]} />
                <ConfigProvider theme={{ components: { List: { paddingContentHorizontal: 0 } } }} renderEmpty={() => <Empty description={"The role doesn't have any privileges"}></Empty>}>
                    <List
                        split={false}
                        size="small"
                        bordered={false}
                        loading={progress}
                        dataSource={rolePrivileges}
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