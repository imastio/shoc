import { EditOutlined } from "@ant-design/icons";
import { Button, Card, Descriptions, Skeleton } from "antd";
import { italic } from "@/extended/typography";
import { useState } from "react";
import ApplicationTokensUpdateModel from "./application-tokens-update-modal";
import { tokenExpirationsMap, tokenUsageTypesMap } from "@/well-known/applications";

const maybeSeconds = (value: string) => {
    if(value === null || value === undefined){
        return italic("System Default");
    }

    return `${value}s`;
}

export default function ApplicationTokensCard({ application = {}, loading = false, onUpdate = () => { } }: any) {

    const [updateActive, setUpdateActive] = useState(false);

    return <Card>
        <ApplicationTokensUpdateModel application={application} open={updateActive} onClose={() => setUpdateActive(false)} onSuccess={({id}: {id: string}) => onUpdate(id)} />
        <Skeleton loading={loading} paragraph={{ rows: 5 }}>
            <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="Tokens Configuration" extra={[
                <Button key="edit" size="middle" type="text" onClick={() => setUpdateActive(true)} disabled={loading || !application}>
                    <EditOutlined />
                </Button>
            ]}>
                <Descriptions.Item key="accessTokenLifetime" label="Access Token Lifetime">{maybeSeconds(application.accessTokenLifetime)}</Descriptions.Item>
                <Descriptions.Item key="absoluteRefreshTokenLifetime" label="Absolute Refresh Token Lifetime">{maybeSeconds(application.absoluteRefreshTokenLifetime)}</Descriptions.Item>
                <Descriptions.Item key="slidingRefreshTokenLifetime" label="Sliding Refresh Token Lifetime">{maybeSeconds(application.slidingRefreshTokenLifetime)}</Descriptions.Item>
                <Descriptions.Item key="refreshTokenUsage" label="Refresh Token Usage">{tokenUsageTypesMap[application.refreshTokenUsage] || application.refreshTokenUsage || italic("Not Provided")}</Descriptions.Item>
                <Descriptions.Item key="refreshTokenExpiration" label="Refresh Token Expiration">{tokenExpirationsMap[application.refreshTokenExpiration] || application.refreshTokenExpiration || italic("Not Provided")}</Descriptions.Item>
            </Descriptions>
        </Skeleton>
    </Card>
}