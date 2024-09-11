"use client"

import { DownOutlined, EditOutlined, ReloadOutlined, SecurityScanOutlined } from "@ant-design/icons";
import { App, Button, Col, Descriptions, Dropdown, Row, Tabs } from "antd";
import { useCallback, useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";
import { localDateTime } from "@/extended/format";
import Link from "antd/es/typography/Link";
import WorkspaceClustersClient from "@/clients/shoc/cluster/workspace-clusters-client";
import ClusterStatus from "@/components/cluster/cluster-status";
import ClusterUpdateModal from "./cluster-update-modal";
import ClusterConfigurationUpdateModal from "./cluster-configuration-update-modal";
import ClusterDeleteModal from "./cluster-delete-modal";
import { clusterStatusesMap, clusterTypesMap } from "@/well-known/clusters";

export default function SingleClusterClientPage() {

    const params = useParams();
    const router = useRouter();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [updateActive, setUpdateActive] = useState(false);
    const [updateConfigurationActive, setUpdateConfigurationActive] = useState(false);
    const [item, setItem] = useState<any>({});
    const [deleteActive, setDeleteActive] = useState(false);
    const [fatalError, setFatalError] = useState<any>(null);
    const { notification } = App.useApp();

    const workspaceId = params?.workspaceId as string || '';
    const clusterId = params?.id as string || '';

    const load = useCallback(async (workspaceId: string, id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(WorkspaceClustersClient).getExtendedById(token, workspaceId, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setItem(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        if(workspaceId && clusterId){
            load(workspaceId, clusterId);
        }
    }, [load, workspaceId, clusterId])


    const dangerMenuItems = [
        {
            key: "delete",
            label: "Delete",
            disabled: progress || !item.id,
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
                <PageContainer fluid title={item.name || "Cluster Details"}
                    tags={[ <ClusterStatus key="status-tag" status={item.status || 'unknown'} /> ]}
                    extra={[
                        <Button key="edit" title="Edit" type="default" disabled={progress} onClick={() => setUpdateActive(true)}>
                            <EditOutlined />
                        </Button>,
                        <Button key="edit-config" title="Edit configuration" type="default" disabled={progress} onClick={() => setUpdateConfigurationActive(true)}>
                        <SecurityScanOutlined />
                    </Button>,
                        <Button key="reload" title="Reload" type="default" disabled={progress} onClick={() => load(workspaceId, clusterId)}><ReloadOutlined /></Button>,
                        <Dropdown key="danger-zone" disabled={progress || !item.id} menu={{ onClick: menuClickHandler, items: dangerMenuItems }}>
                            <Button danger title="Dangerous operatoins" type="dashed">Danger Zone <DownOutlined /></Button>
                        </Dropdown>
                    ]}
                    content={<>
                        <ClusterUpdateModal
                            open={updateActive}
                            existing={item}
                            onClose={() => setUpdateActive(false)}
                            onSuccess={() => load(workspaceId, clusterId)}
                        />
                        <ClusterConfigurationUpdateModal
                            open={updateConfigurationActive}
                            existing={item}
                            onClose={() => setUpdateConfigurationActive(false)}
                            onSuccess={() => load(workspaceId, clusterId)}
                        />
                        <ClusterDeleteModal
                            open={deleteActive}
                            existing={item}
                            onClose={() => setDeleteActive(false)}
                            onSuccess={() => {
                                notification.success({ message: `The cluster '${item.name}' was successfully deleted` })
                                router.replace("/clusters");
                            }}
                        />
                        <Row gutter={16}>
                            <Col span={24}>
                                <Descriptions>
                                    <Descriptions.Item label="Name">{item.name}</Descriptions.Item>
                                    <Descriptions.Item label="Status">{clusterStatusesMap[item.status] || item.status}</Descriptions.Item>
                                    <Descriptions.Item label="Type">{clusterTypesMap[item.type] || item.type}</Descriptions.Item>
                                    <Descriptions.Item label="Workspace"><Link href={`/workspaces/${item.workspaceId}`}>{item.workspaceName}</Link></Descriptions.Item>
                                    <Descriptions.Item label="Created">{localDateTime(item.created)}</Descriptions.Item>
                                    <Descriptions.Item label="Updated">{localDateTime(item.updated)}</Descriptions.Item>
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
