import { EditOutlined } from "@ant-design/icons";
import { Button, Card, Descriptions, Skeleton } from "antd";
import { italic } from "@/extended/typography";
import { useState } from "react";
import ApplicationGeneralUpdateModal from "./application-general-update-modal";


const maybe = (value: string, yes?: boolean, no?: boolean) => {
    if (value === null || value === undefined) {
        return italic("System Default");
    }

    return value ? yes || 'Yes' : no || 'No'
}

export default function ApplicationGeneralCard({ application = {}, loading = false, onUpdate = () => { } }: any) {

    const [updateActive, setUpdateActive] = useState(false);

    return <Card>
        <ApplicationGeneralUpdateModal application={application} open={updateActive} onClose={() => setUpdateActive(false)} onSuccess={({ id } : { id: string }) => onUpdate(id)} />
        <Skeleton loading={loading} paragraph={{ rows: 5 }}>
            <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="General Configuration" extra={[
                <Button key="edit" size="middle" type="text" onClick={() => setUpdateActive(true)} disabled={loading || !application}>
                    <EditOutlined />
                </Button>
            ]}>
                <Descriptions.Item key="secretRequired" label="Secret Required">{maybe(application.secretRequired)}</Descriptions.Item>
                <Descriptions.Item key="pkceRequired" label="PKCE Required">{maybe(application.pkceRequired)}</Descriptions.Item>
                <Descriptions.Item key="allowOfflineAccess" label="Allow Offline Access">{maybe(application.allowOfflineAccess)}</Descriptions.Item>
                <Descriptions.Item key="allowedScopes" label="Allowed Scopes">{application.allowedScopes || italic("Not Provided")}</Descriptions.Item>
                <Descriptions.Item key="allowedGrantTypes" label="Allowed Grant Types">{application.allowedGrantTypes || italic("Not Provided")}</Descriptions.Item>
            </Descriptions>
        </Skeleton>
    </Card>
}