"use client"

import { DownOutlined } from "@ant-design/icons";
import { App, Button, Col, Descriptions, Dropdown, Row, Tabs } from "antd";
import { useCallback, useEffect, useState } from "react";
import UserGroupDeleteModal from "./user-group-delete-modal";
import UserGroupDetailsTab from "./user-group-details-tab";
import { useParams, useRouter } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import UserGroupsClient from "@/clients/shoc/identity/user-groups-client";
import { localDateTime } from "@/extended/format";
import PageContainer from "@/components/general/page-container";
import { ObjectContainer } from "@/components/general/object-container";

export default function SingleUserGroupClientPage() {

    const params = useParams();
    const router = useRouter();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [group, setGroup] = useState<any>({});
    const [deleteActive, setDeleteActive] = useState(false);
    const [fatalError, setFatalError] = useState<any | null>(null);
    const { notification } = App.useApp();

    const groupId = params?.id as string || '';

    const load = useCallback(async (id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(UserGroupsClient).getById(token, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setGroup(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        if (groupId) {
            load(groupId);
        }
    }, [load, groupId])


    const dangerMenuItems = [
        {
            key: "delete",
            label: "Delete",
            disabled: progress || !group.id,
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
                <PageContainer fluid title={group.name || "User Group Details"}
                    extra={[
                        <Dropdown key="danger-zone" disabled={progress || !group.id} menu={{ onClick: menuClickHandler, items: dangerMenuItems }}>
                            <Button danger title="Dangerous operatoins" type="dashed">Danger Zone <DownOutlined /></Button>
                        </Dropdown>
                    ]}
                    tabProps={{
                        items: [
                            {
                                key: "1",
                                label: "Details",
                                children: <UserGroupDetailsTab groupId={groupId} loading={progress} />
                            }
                        ]
                    }}
                    tabList={[
                        {
                            tabKey: "1"
                        }
                    ]}
                    content={<Row gutter={16}>
                    <Col span={24}>
                        <Descriptions>
                            <Descriptions.Item label="Name">{group.name}</Descriptions.Item>
                            <Descriptions.Item label="Created">{localDateTime(group.created)}</Descriptions.Item>
                            <Descriptions.Item label="Updated">{localDateTime(group.updated)}</Descriptions.Item>
                        </Descriptions>
                    </Col>
                </Row>}
                >
                    <UserGroupDeleteModal
                        open={deleteActive}
                        groupId={group.id}
                        onClose={() => setDeleteActive(false)}
                        onSuccess={() => {
                            notification.success({ message: `The group '${group.name}' was successfully deleted` })
                            router.replace("/user-groups");
                        }}
                    />
                </PageContainer>
            </ObjectContainer>
        </>
    )
}
