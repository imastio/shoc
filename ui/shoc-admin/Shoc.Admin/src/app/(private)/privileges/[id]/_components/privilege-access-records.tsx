import { PlusOutlined } from "@ant-design/icons";
import { Button, Card, Descriptions, Empty, Skeleton, Tag } from "antd";
import { useCallback, useEffect, useState } from "react";
import { PrivilegeAccessAddModal } from "./privilege-access-add-modal";
import { PrivilegeAccessDeleteModal } from "./privilege-access-delete-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import PrivilegeAccessesClient from "@/clients/shoc/identity/privilege-accesses-client";

export function PrivilegeAccessRecords({privilegeId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [privilegeAccesses, setPrivilegeAccesses] = useState<any[]>([]);
    const [addingActive, setAddingActive] = useState(false);
    const [deletingItem, setDeletingItem] = useState(null);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(PrivilegeAccessesClient).get(token, privilegeId));
        setProgress(false);

        if (result.error) {
            return;
        }

        setPrivilegeAccesses(result.payload)
    }, [withToken, privilegeId])

    useEffect(() => {
        if (!privilegeId) {
            return;
        }
        load();
    }, [load, privilegeId]);

    return <>
        <PrivilegeAccessAddModal
            key="add-modal"
            privilegeId={privilegeId}
            onClose={() => setAddingActive(false)}
            granted={privilegeAccesses}
            open={addingActive}
            onSuccess={() => {
                load();
                onUpdate();
            }}
        />
        <PrivilegeAccessDeleteModal
            key="delete-modal"
            privilegeId={privilegeId}
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
                { (!privilegeAccesses || privilegeAccesses.length === 0) &&  <Empty description={"The privilege does not have any accesses"} /> }
                {privilegeAccesses && privilegeAccesses.length > 0 && <>
                    {privilegeAccesses.map((item, index) => <Tag 
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