"use client"

import { DownOutlined, EditOutlined, ReloadOutlined } from "@ant-design/icons";
import { App, Button, Col, Descriptions, Dropdown, Row, Tabs } from "antd";
import { useCallback, useEffect, useState } from "react";
import RoleDetailsTab from "./role-details-tab";
import RoleUpdateModal from "./role-update-modal";
import RoleDeleteModal from "./role-delete-modal";
import { useParams, useRouter } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import RolesClient from "@/clients/shoc/identity/roles-client";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";
import { localDateTime } from "@/extended/format";

export default function SingleRoleClientPage() {

    const params = useParams();
    const router = useRouter();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [updateActive, setUpdateActive] = useState(false);
    const [role, setRole] = useState<any>({});
    const [deleteActive, setDeleteActive] = useState(false);
    const [fatalError, setFatalError] = useState<any>(null);
    const { notification } = App.useApp();

    const roleId = params?.id as string || '';

    const load = useCallback(async (id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(RolesClient).getById(token, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setRole(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        load(roleId);
    }, [load, roleId])


    const dangerMenuItems = [
        {
            key: "delete",
            label: "Delete",
            disabled: progress || !role.id,
            danger: true
        }
    ]

    const menuClickHandler = ({ key }: { key: string }) => {
        switch (key) {
            case "delete":
                setDeleteActive(true);
                break;
            default:
                return;
        }
    }

    return (
        <>
            <ObjectContainer loading={progress} fatalError={fatalError}>
                <PageContainer fluid title={role.name || "Role Details"}
                    extra={[
                        <Button key="edit" title="Edit role" type="default" disabled={progress} onClick={() => setUpdateActive(true)}>
                            <EditOutlined />
                        </Button>,
                        <Button key="reload" title="Reload" type="default" disabled={progress} onClick={() => load(roleId)}><ReloadOutlined /></Button>,
                        <Dropdown key="danger-zone" disabled={progress || !role.id} menu={{ onClick: menuClickHandler, items: dangerMenuItems }}>
                            <Button danger title="Dangerous operatoins" type="dashed">Danger Zone <DownOutlined /></Button>
                        </Dropdown>
                    ]}
                    tabProps={{
                        items: [
                            {
                                key: "1",
                                label: "Details",
                                children: <RoleDetailsTab roleId={roleId} loading={progress} />
                            }
                        ]
                    }}
                    tabList={[
                        {
                            tabKey: "1"
                        }
                    ]}
                    content={<>
                        <RoleUpdateModal
                            open={updateActive}
                            role={role}
                            onClose={() => setUpdateActive(false)}
                            onSuccess={() => load(roleId)}
                        />
                        <RoleDeleteModal
                            open={deleteActive}
                            roleId={role.id}
                            onClose={() => setDeleteActive(false)}
                            onSuccess={() => {
                                notification.success({ message: `The role '${role.name}' was successfully deleted` })
                                router.replace("/roles");
                            }}
                        />
                        <Row gutter={16}>
                            <Col span={24}>
                                <Descriptions>
                                    <Descriptions.Item label="Name">{role.name}</Descriptions.Item>
                                    <Descriptions.Item label="Description">{role.description}</Descriptions.Item>
                                    <Descriptions.Item label="Created">{localDateTime(role.created)}</Descriptions.Item>
                                    <Descriptions.Item label="Updated">{localDateTime(role.updated)}</Descriptions.Item>
                                </Descriptions>
                            </Col>
                        </Row>
                    </>}
                >
            </PageContainer>
        </ObjectContainer >
        </>
    )
}
