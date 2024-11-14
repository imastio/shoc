import { PlusOutlined } from "@ant-design/icons";
import { Button, Card, Descriptions, Empty, Skeleton, Tag } from "antd";
import { useCallback, useEffect, useState } from "react";
import { UserGroupAccessAddModal } from "./user-group-access-add-modal";
import { UserGroupAccessDeleteModal } from "./user-group-access-delete-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import UserGroupAccessesClient from "@/clients/shoc/identity/user-group-accesses-client";

export function UserGroupAccessRecords({groupId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [userGroupAccesses, setUserGroupAccesses] = useState<any[]>([]);
    const [addingActive, setAddingActive] = useState(false);
    const [deletingItem, setDeletingItem] = useState(null);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UserGroupAccessesClient).get(token, groupId));
        setProgress(false);

        if (result.error) {
            return;
        }

        setUserGroupAccesses(result.payload)
    }, [withToken, groupId])

    useEffect(() => {
        if (!groupId) {
            return;
        }
        load();
    }, [load, groupId]);

    return <>
        <UserGroupAccessAddModal
            key="add-modal"
            groupId={groupId}
            onClose={() => setAddingActive(false)}
            granted={userGroupAccesses}
            open={addingActive}
            onSuccess={() => {
                load();
                onUpdate();
            }}
        />
        <UserGroupAccessDeleteModal
            key="delete-modal"
            groupId={groupId}
            existing={deletingItem}
            onClose={() => setDeletingItem(null)}
            open={deletingItem}
            onSuccess={(entity: any) => {
                load();
                onUpdate(entity);
            }}
        />
        <Card>
            <Skeleton loading={progress} paragraph={{ rows: 5 }}>
                <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="Accesses" extra={[
                    <Button key="add-button" type="text" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)} />
                ]} />
                { (!userGroupAccesses || userGroupAccesses.length === 0) &&  <Empty description={"The user group does not have any accesses"} /> }
                {userGroupAccesses && userGroupAccesses.length > 0 && <>
                    {userGroupAccesses.map((item, index) => <Tag 
                        style={{margin: 4}} 
                        closable 
                        onClose={(e) => { 
                            e.preventDefault(); 
                            setDeletingItem(item);
                        }} 
                        key={index}
                        >
                            {item.access}
                        </Tag>)}
                </>}
            </Skeleton>
        </Card>
    </>
}