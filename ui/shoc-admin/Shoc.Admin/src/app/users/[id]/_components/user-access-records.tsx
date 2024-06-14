import { PlusOutlined } from "@ant-design/icons";
import { Button, Card, Descriptions, Empty, Skeleton, Tag } from "antd";
import { useCallback, useEffect, useState } from "react";
import { UserAccessAddModal } from "./user-access-add-modal";
import { UserAccessDeleteModal } from "./user-access-delete-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import UserAccessesClient from "@/clients/shoc/identity/user-accesses-client";

export function UserAccessRecords({userId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [userAccesses, setUserAccesses] = useState([]);
    const [addingActive, setAddingActive] = useState(false);
    const [deletingItem, setDeletingItem] = useState(null);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UserAccessesClient).get(token, userId));
        setProgress(false);

        if (result.error) {
            return;
        }

        setUserAccesses(result.payload)
    }, [withToken, userId])

    useEffect(() => {
        if (!userId) {
            return;
        }
        load();
    }, [load, userId]);

    return <>
        <UserAccessAddModal
            key="add-modal"
            userId={userId}
            onClose={() => setAddingActive(false)}
            granted={userAccesses}
            open={addingActive}
            onSuccess={() => {
                load();
                onUpdate();
            }}
        />
        <UserAccessDeleteModal
            key="delete-modal"
            userId={userId}
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
                { (!userAccesses || userAccesses.length === 0) &&  <Empty description={"The user does not have any accesses"} /> }
                {userAccesses && userAccesses.length > 0 && <>
                    {userAccesses.map((item: any, index) => <Tag 
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