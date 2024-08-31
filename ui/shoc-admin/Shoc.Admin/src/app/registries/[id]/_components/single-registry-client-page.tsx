"use client"

import { DownOutlined, EditOutlined, ReloadOutlined } from "@ant-design/icons";
import { App, Button, Col, Descriptions, Dropdown, Row, Tabs } from "antd";
import { useCallback, useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";
import { localDateTime } from "@/extended/format";
import RegistriesClient from "@/clients/shoc/registry/registries-client";
import RegistryUpdateModal from "./registry-update-modal";
import RegistryDeleteModal from "./registry-delete-modal";
import { registryProviderTypesMap, registryStatusesMap, registryUsageScopes, registryUsageScopesMap } from "@/well-known/registries";
import RegistryStatus from "@/components/registry/registry-status";
import Link from "antd/es/typography/Link";
import { RegistryCredentialsTable } from "./registry-credentials-table";

export default function SingleRegistryClientPage() {

    const params = useParams();
    const router = useRouter();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [updateActive, setUpdateActive] = useState(false);
    const [item, setItem] = useState<any>({});
    const [deleteActive, setDeleteActive] = useState(false);
    const [fatalError, setFatalError] = useState<any>(null);
    const { notification } = App.useApp();

    const registryId = params?.id as string || '';

    const load = useCallback(async (id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(RegistriesClient).getById(token, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setItem(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        if(registryId){
            load(registryId);
        }
    }, [load, registryId])


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
                <PageContainer fluid title={item.name || "Registry Details"}
                    tags={[ <RegistryStatus key="status-tag" status={item.status || 'unknown'} /> ]}
                    extra={[
                        <Button key="edit" title="Edit role" type="default" disabled={progress} onClick={() => setUpdateActive(true)}>
                            <EditOutlined />
                        </Button>,
                        <Button key="reload" title="Reload" type="default" disabled={progress} onClick={() => load(registryId)}><ReloadOutlined /></Button>,
                        <Dropdown key="danger-zone" disabled={progress || !item.id} menu={{ onClick: menuClickHandler, items: dangerMenuItems }}>
                            <Button danger title="Dangerous operatoins" type="dashed">Danger Zone <DownOutlined /></Button>
                        </Dropdown>
                    ]}
                    tabProps={{
                        destroyInactiveTabPane: true,
                        items: [
                            {
                                key: "1",
                                label: "Credentials",
                                children: <RegistryCredentialsTable registryId={registryId} loading={progress} />
                            }
                        ]
                    }}
                    tabList={[
                        {
                            tabKey: "1",
                        }
                    ]}
                    content={<>
                        <RegistryUpdateModal
                            open={updateActive}
                            existing={item}
                            onClose={() => setUpdateActive(false)}
                            onSuccess={() => load(registryId)}
                        />
                        <RegistryDeleteModal
                            open={deleteActive}
                            registryId={registryId}
                            onClose={() => setDeleteActive(false)}
                            onSuccess={() => {
                                notification.success({ message: `The registry '${item.name}' was successfully deleted` })
                                router.replace("/registries");
                            }}
                        />
                        <Row gutter={16}>
                            <Col span={24}>
                                <Descriptions>
                                    <Descriptions.Item label="Name">{item.name}</Descriptions.Item>
                                    <Descriptions.Item label="Display Name">{item.displayName}</Descriptions.Item>
                                    <Descriptions.Item label="Status">{registryStatusesMap[item.status] || item.status}</Descriptions.Item>
                                    <Descriptions.Item label="Provider">{registryProviderTypesMap[item.provider] || item.provider}</Descriptions.Item>
                                    <Descriptions.Item label="Usage Scope">{registryUsageScopesMap[item.usageScope] || item.usageScope}</Descriptions.Item>
                                    <Descriptions.Item label="Registry Host">{item.registry}</Descriptions.Item>
                                    <Descriptions.Item label="Namespace">{item.namespace}</Descriptions.Item>
                                    <Descriptions.Item label="Workspace">{item.workspaceId ? <Link href={`/workspaces/${item.workspaceId}`}>Workspace</Link> : 'None'}</Descriptions.Item>
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
