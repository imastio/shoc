"use client"

import { DownOutlined, EditOutlined, ReloadOutlined } from "@ant-design/icons";
import { App, Button, Col, Descriptions, Dropdown, Row, Tabs } from "antd";
import { useCallback, useEffect, useState } from "react";
import WorkspaceDetailsTab from "./workspace-members-tab";
import { useParams, useRouter } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";
import { localDateTime } from "@/extended/format";
import WorkspacesClient from "@/clients/shoc/workspace/workspaces-client";
import WorkspaceDeleteModal from "./workspace-delete-modal";
import WorkspaceUpdateModal from "./workspace-update-modal";
import { workspaceStatusesMap, workspaceTypesMap } from "@/well-known/workspaces";
import WorkspaceMembersTab from "./workspace-members-tab";

export default function SingleWorkspaceClientPage() {

    const params = useParams();
    const router = useRouter();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [updateActive, setUpdateActive] = useState(false);
    const [workspace, setWorkspace] = useState<any>({});
    const [deleteActive, setDeleteActive] = useState(false);
    const [fatalError, setFatalError] = useState<any>(null);
    const { notification } = App.useApp();

    const workspaceId = params?.id as string || '';

    const load = useCallback(async (id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(WorkspacesClient).getById(token, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setWorkspace(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        if(workspaceId){
            load(workspaceId);
        }
    }, [load, workspaceId])


    const dangerMenuItems = [
        {
            key: "delete",
            label: "Delete",
            disabled: progress || !workspace.id,
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
                <PageContainer fluid title={workspace.name || "Workspace Details"}
                    extra={[
                        <Button key="edit" title="Edit role" type="default" disabled={progress} onClick={() => setUpdateActive(true)}>
                            <EditOutlined />
                        </Button>,
                        <Button key="reload" title="Reload" type="default" disabled={progress} onClick={() => load(workspaceId)}><ReloadOutlined /></Button>,
                        <Dropdown key="danger-zone" disabled={progress || !workspace.id} menu={{ onClick: menuClickHandler, items: dangerMenuItems }}>
                            <Button danger title="Dangerous operatoins" type="dashed">Danger Zone <DownOutlined /></Button>
                        </Dropdown>
                    ]}
                    tabProps={{
                        items: [
                            {
                                key: "1",
                                label: "Members",
                                children: <WorkspaceMembersTab workspaceId={workspaceId} loading={progress} />
                            }
                        ]
                    }}
                    tabList={[
                        {
                            tabKey: "1"
                        }
                    ]}
                    content={<>
                        <WorkspaceUpdateModal
                            open={updateActive}
                            existing={workspace}
                            onClose={() => setUpdateActive(false)}
                            onSuccess={() => load(workspaceId)}
                        />
                        <WorkspaceDeleteModal
                            open={deleteActive}
                            workspaceId={workspaceId}
                            onClose={() => setDeleteActive(false)}
                            onSuccess={() => {
                                notification.success({ message: `The workspace '${workspace.name}' was successfully deleted` })
                                router.replace("/workspaces");
                            }}
                        />
                        <Row gutter={16}>
                            <Col span={24}>
                                <Descriptions>
                                    <Descriptions.Item label="Name">{workspace.name}</Descriptions.Item>
                                    <Descriptions.Item label="Title">{workspace.title}</Descriptions.Item>
                                    <Descriptions.Item label="Type">{workspaceTypesMap[workspace.type] || workspace.type}</Descriptions.Item>
                                    <Descriptions.Item label="Status">{workspaceStatusesMap[workspace.status] || workspace.status}</Descriptions.Item>
                                    <Descriptions.Item label="Created">{localDateTime(workspace.created)}</Descriptions.Item>
                                    <Descriptions.Item label="Updated">{localDateTime(workspace.updated)}</Descriptions.Item>
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
