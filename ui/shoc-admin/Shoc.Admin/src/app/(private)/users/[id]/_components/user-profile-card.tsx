import { EditOutlined } from "@ant-design/icons";
import { Button, Card, Descriptions, Skeleton, Typography } from "antd";
import { localDate } from "@/extended/format";
import { useCallback, useEffect, useState } from "react";
import UserProfileUpdateModal from "./user-profile-update-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import { gendersMap } from "@/well-known/user-details";

export default function UserProfileCard({ userId, loading = false } : { userId: string, loading: boolean }) {

    const [progress, setProgress] = useState(true);
    const { withToken } = useApiAuthentication();
    const [data, setData] = useState<any>(null);
    const [updatingObject, setUpdatingObject] = useState(null);

    const load = useCallback(async (id: string) => {

        setProgress(true);
        const result = await withToken((token: string) => selfClient(UsersClient).getProfileById(token, id))
        setProgress(false);

        if (result.error) {
            return;
        }

        setData(result.payload || {});

    }, [withToken])

    useEffect(() => {
        if (!userId) {
            return;
        }

        load(userId);
    }, [userId, load]);

    const notProvided = <Typography.Text italic>Not Provided</Typography.Text>

    return <>
        <UserProfileUpdateModal userId={userId} open={updatingObject} onClose={() => setUpdatingObject(null)} onSuccess={setData} data={updatingObject} />
        <Card>
            <Skeleton loading={progress} paragraph={{ rows: 10 }}>
                <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="Personal" extra={[
                    <Button key="edit" size="middle" type="text" onClick={() => setUpdatingObject(data)} disabled={progress || loading || !data}><EditOutlined /></Button>
                ]}>
                    <Descriptions.Item key="firstName" label="First Name">{data?.firstName || notProvided}</Descriptions.Item>
                    <Descriptions.Item key="lastName" label="Last Name">{data?.lastName || notProvided}</Descriptions.Item>
                    <Descriptions.Item key="phone" label="Phone">{data?.phone || notProvided}</Descriptions.Item>
                    <Descriptions.Item key="gender" label="Gender">{data?.gender ? gendersMap[data?.gender] : notProvided}</Descriptions.Item>
                    <Descriptions.Item key="birthDate" label="Birth date">{localDate(data?.birthDate) || notProvided}</Descriptions.Item>
                    <Descriptions.Item key="country" label="Country">{data?.country || notProvided}</Descriptions.Item>
                    {data?.state && <Descriptions.Item key="state" label="State">{data?.state || notProvided}</Descriptions.Item>}
                    {data?.city && <Descriptions.Item key="city" label="City">{data?.city || notProvided}</Descriptions.Item>}
                    {data?.postal && <Descriptions.Item key="postal" label="Postal Code">{data?.postal || notProvided}</Descriptions.Item>}
                    {data?.address1 && <Descriptions.Item key="address1" label="Address 1">{data?.address1 || notProvided}</Descriptions.Item>}
                    {data?.address2 && <Descriptions.Item key="address2" label="Address 2">{data?.address2 || notProvided}</Descriptions.Item>}
                </Descriptions>
            </Skeleton>

        </Card>
    </>
}