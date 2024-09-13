"use client"

import { DownOutlined, EditOutlined, ReloadOutlined } from "@ant-design/icons";
import { App, Button, Col, Descriptions, Dropdown, Row, Tabs } from "antd";
import { useCallback, useEffect, useState } from "react";
import PrivilegeDeleteModal from "./privilege-delete-modal";
import PrivilegeDetailsTab from "./privilege-details-tab";
import PrivilegeUpdateModal from "./privilege-update-modal";
import { useParams, useRouter } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import PrivilegesClient from "@/clients/shoc/identity/privileges-client";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";
import { localDateTime } from "@/extended/format";

export default function SinglePrivilegeClientPage() {

    const params = useParams();
    const router = useRouter();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [updateActive, setUpdateActive] = useState(false);
    const [privilege, setPrivilege] = useState<any>({});
    const [deleteActive, setDeleteActive] = useState(false);
    const [fatalError, setFatalError] = useState<any>(null);
    const { notification } = App.useApp();

    const privilegeId = params?.id as string || '';

    const load = useCallback(async (id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(PrivilegesClient).getById(token, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setPrivilege(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        if (privilegeId) {
            load(privilegeId);
        }
    }, [load, privilegeId])


    const dangerMenuItems = [
        {
            key: "delete",
            label: "Delete",
            disabled: progress || !privilege.id,
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
                <PageContainer fluid title={privilege.name || "Privilege"}
                    extra={[
                        <Button key="edit" title="Edit privilege" type="default" disabled={progress} onClick={() => setUpdateActive(true)}>
                            <EditOutlined />
                        </Button>,
                        <Button key="reload" title="Reload" type="default" disabled={progress} onClick={() => load(privilegeId)}><ReloadOutlined /></Button>,
                        <Dropdown key="danger-zone" disabled={progress || !privilege.id} menu={{ onClick: menuClickHandler, items: dangerMenuItems }}>
                            <Button danger title="Dangerous operatoins" type="dashed">Danger Zone <DownOutlined /></Button>
                        </Dropdown>
                    ]}
                    tabProps={{
                        items: [
                            {
                                key: "1",
                                label: "Details",
                                children: <PrivilegeDetailsTab privilegeId={privilegeId} loading={progress} />
                            }
                        ]
                    }}
                    tabList={[
                        {
                            tabKey: "1"
                        }
                    ]}
                    content={
                        <><PrivilegeUpdateModal
                            open={updateActive}
                            privilege={privilege}
                            onClose={() => setUpdateActive(false)}
                            onSuccess={() => load(privilegeId)}
                        />
                            <PrivilegeDeleteModal
                                open={deleteActive}
                                privilegeId={privilege.id}
                                onClose={() => setDeleteActive(false)}
                                onSuccess={() => {
                                    notification.success({ message: `The privilege '${privilege.name}' was successfully deleted` })
                                    router.replace("/privileges");
                                }}
                            />
                            <Row gutter={16}>
                                <Col span={24}>
                                    <Descriptions>
                                        <Descriptions.Item label="Name">{privilege.name}</Descriptions.Item>
                                        <Descriptions.Item label="Category">{privilege.category}</Descriptions.Item>
                                        <Descriptions.Item label="Description">{privilege.description}</Descriptions.Item>
                                        <Descriptions.Item label="Created">{localDateTime(privilege.created)}</Descriptions.Item>
                                        <Descriptions.Item label="Updated">{localDateTime(privilege.updated)}</Descriptions.Item>
                                    </Descriptions>
                                </Col>
                            </Row></>
                    }
                >

                </PageContainer>
            </ObjectContainer>
        </>
    )
}
