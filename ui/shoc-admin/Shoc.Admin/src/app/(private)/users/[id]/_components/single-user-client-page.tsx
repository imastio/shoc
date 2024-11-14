"use client"

import { DownOutlined, EditOutlined, LockOutlined, ReloadOutlined, UnlockOutlined } from "@ant-design/icons";
import { Button, Col, Descriptions, Dropdown, Row, Tabs, Tag } from "antd";
import React, { useCallback, useEffect, useState } from "react";
import dayjs from "@/extended/time";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import { useParams } from "next/navigation";
import PageContainer from "@/components/general/page-container";
import { localDateTime } from "@/extended/format";
import { ObjectContainer } from "@/components/general/object-container";
import UserAvatar from "./user-avatar";
import UserDetailsTab from "./user-details-tab";
import UserLockModal from "./user-lock-modal";
import UserUnlockModal from "./user-unlock-modal";
import UserTypeUpdateModal from "./user-type-update-modal";
import UserPasswordUpdateModal from "./user-password-update-modal";
import UserUpdateModal from "./user-update-modal";

export default function SingleUserClientPage() {

    const params = useParams();
    const { withToken } = useApiAuthentication();

    const [user, setUser] = useState<any>({});
    const [progress, setProgress] = useState(true);
    const [updatePasswordActive, setUpdatePasswordActive] = useState(false);
    const [updateActive, setUpdateActive] = useState(false);
    const [updateTypeActive, setUpdateTypeActive] = useState(false);
    const [lockUserActive, setLockUserActive] = useState(false);
    const [unlockUserActive, setUnlockUserActive] = useState(false);
    const [fatalError, setFatalError] = useState<any>(null);

    const userId = params?.id as string || '';
    
    const load = useCallback(async (id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(UsersClient).getById(token, id))

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setUser(result.payload || {});

    }, [withToken])

    useEffect(() => {
        if(userId){
            load(userId);
        }
    }, [userId, load]);

    const verified = user.id && (user.emailVerified || user.phoneVerified);
    const locked = user.lockedUntil && dayjs.utc(user.lockedUntil).isAfter(dayjs.utc());

    const dangerMenuItems = [
        {
            key: "update-type",
            label: "Update Type",
            danger: true
        },
        {
            key: "update-password",
            label: "Update Password",
            danger: true
        },
        {
            key: "delete",
            label: "Delete",
            danger: true
        }
    ]

    const menuClickHandler = ({ key }: { key: string }) => {
        switch (key) {
            case "update-password":
                setUpdatePasswordActive(true);
                break;
            case "update-type":
                setUpdateTypeActive(true);
                break;
            case "delete":
                break;
            default:
                return;
        }
    }

    return (
        <>
            <ObjectContainer loading={progress} fatalError={fatalError}>
                <PageContainer fluid title={user.fullName || "User Profile"}
                    loading={progress}
                    tags={[verified ? <Tag key="success" color="success">Verified</Tag> : <Tag key="warning" color="warning">Not verified</Tag>]}
                    extra={[
                        <Button key="edit" title="Edit user" type="default" disabled={progress} onClick={() => setUpdateActive(true)}>
                            <EditOutlined />
                        </Button>,
                        <Button key="lockout" title={`${locked ? 'Unlock' : 'Lock'} user`} type="default" disabled={progress} onClick={() => locked ? setUnlockUserActive(true) : setLockUserActive(true)}>
                            {locked ? <UnlockOutlined /> : <LockOutlined />}
                        </Button>,
                        <Button key="reload" title="Reload" type="default" disabled={progress} onClick={() => load(userId)}><ReloadOutlined /></Button>,
                        <Dropdown key="danger-zone" disabled={progress || !user.id} menu={{ onClick: menuClickHandler, items: dangerMenuItems }}>
                            <Button danger title="Dangerous operatoins" type="dashed">Danger Zone <DownOutlined /></Button>
                        </Dropdown>
                    ]}
                    tabProps={{ items: [
                        {
                            key: "1",
                            label: "Details",
                            children: <UserDetailsTab userId={userId} loading={progress} />
                        }
                    ] }}
                    tabList={[{
                        tabKey: "1"
                    }]}
                    content={<Row gutter={16} align="middle">
                    <Col lg={4} md={24} style={{ textAlign: 'center' }}>
                        <UserAvatar
                            userId={user?.id}
                            meta={`${user?.fullName ? user.fullName[0] : 'U'}`}
                        />
                    </Col>
                    <Col lg={20} md={24}>
                        <Descriptions column={{ xxl: 3, xl: 3, lg: 2, md: 2, sm: 2, xs: 1 }}>
                            <Descriptions.Item label="Full Name">{user.fullName}</Descriptions.Item>
                            <Descriptions.Item label="Email">{user.email} </Descriptions.Item>
                            <Descriptions.Item label="Username">{user.username}</Descriptions.Item>
                            <Descriptions.Item label="Type">{user.type}</Descriptions.Item>
                            <Descriptions.Item label="Last Login">{localDateTime(user.lastLogin)}</Descriptions.Item>
                            <Descriptions.Item label="Last IP">{user.lastIp}</Descriptions.Item>
                            <Descriptions.Item label="Timezone">{user.timezone}</Descriptions.Item>
                            <Descriptions.Item label="Failed Attempts">{user.failedAttempts}</Descriptions.Item>
                            <Descriptions.Item label="Created">{localDateTime(user.created)}</Descriptions.Item>
                            <Descriptions.Item label="Updated">{localDateTime(user.updated)}</Descriptions.Item>
                        </Descriptions>
                    </Col>
                </Row>}
                    >

                <UserUpdateModal open={updateActive} onClose={() => setUpdateActive(false)} onSuccess={() => load(userId)} user={user} />
                <UserPasswordUpdateModal open={updatePasswordActive} onClose={() => setUpdatePasswordActive(false)} onSuccess={() => load(userId)} userId={userId} />
                <UserTypeUpdateModal open={updateTypeActive} onClose={() => setUpdateTypeActive(false)} onSuccess={() => load(userId)} user={user} />
                <UserLockModal open={lockUserActive} onClose={() => setLockUserActive(false)} onSuccess={() => load(userId)} userId={userId} />
                <UserUnlockModal open={unlockUserActive} onClose={() => setUnlockUserActive(false)} onSuccess={() => load(userId)} userId={userId} />

                    
                </PageContainer>
            </ObjectContainer>
        </>
    )
}