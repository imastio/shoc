"use client"

import { DeleteOutlined, EditOutlined, ReloadOutlined, SecurityScanOutlined } from "@ant-design/icons";
import { App, Button, Col, Descriptions, Row, Tag } from "antd";
import React, { useCallback, useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";
import { localDateTime } from "@/extended/format";
import { oidcProviderTypesMap } from "@/well-known/oidc-providers";
import OidcProvidersClient from "@/clients/shoc/identity/oidc-providers-client";
import OidcProviderUpdateModal from "./oidc-provider-update-modal";
import OidcProviderClientSecretUpdateModal from "./oidc-provider-client-secret-update-modal";
import OidcProviderDeleteModal from "./oidc-provider-delete-modal";

export default function SingleOidcProviderClientPage() {

    const params = useParams();
    const router = useRouter();
    const { notification } = App.useApp();
    const { withToken } = useApiAuthentication();
    const [provider, setProvider] = useState<any>({});
    const [updateActive, setUpdateActive] = useState(false);
    const [updateSecretActive, setUpdateSecretActive] = useState(false);
    const [deleteActive, setDeleteActive] = useState(false);
    const [progress, setProgress] = useState(true);
    const [fatalError, setFatalError] = useState<any>(null);

    const providerId = params?.id as string || '';

    const load = useCallback(async (id: string) => {

        setProgress(true);
        const result = await withToken((token: string) => selfClient(OidcProvidersClient).getById(token, id))
        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setProvider(result.payload || {});

    }, [withToken])

    useEffect(() => {
        if (providerId) {
            load(providerId);
        }
    }, [providerId, load]);

    return (<>
        <ObjectContainer loading={progress} fatalError={fatalError}>
            <PageContainer fluid
                title={provider.name || provider.code || ''}
                extra={[
                    <Button key="update" disabled={progress} onClick={() => setUpdateActive(true)}><EditOutlined /></Button>,
                    <Button key="update-secret" disabled={progress} onClick={() => setUpdateSecretActive(true)}><SecurityScanOutlined /></Button>,
                    <Button key="refresh" disabled={progress} onClick={() => load(providerId)}><ReloadOutlined /></Button>,
                    <Button key="delete" danger disabled={progress} onClick={() => setDeleteActive(true)}><DeleteOutlined /></Button>
                ]}
                tags={[
                    <Tag key="state" color={provider.disabled ? 'default' : 'success'}>
                        {provider.disabled ? 'Disabled' : 'Enabled'}
                    </Tag>
                ]}
                tabProps={{
                    destroyInactiveTabPane: true,
                    items: [
                        {
                            key: "domains",
                            label: "Domains",
                            children: <p>domains</p>
                        }
                    ]
                }}
                tabList={[
                    {
                        tabKey: "domains"
                    }
                ]}
                content={
                    <Row gutter={16} align="middle" >
                        <Col lg={24} md={24}>
                            <OidcProviderUpdateModal open={updateActive}
                                provider={provider}
                                onClose={() => setUpdateActive(false)}
                                onSuccess={() => load(providerId)} />

                            <OidcProviderClientSecretUpdateModal open={updateSecretActive}
                                provider={provider}
                                onClose={() => setUpdateSecretActive(false)}
                                onSuccess={() => load(providerId)} />

                            <OidcProviderDeleteModal
                                open={deleteActive}
                                providerId={providerId}
                                onClose={() => setDeleteActive(false)}
                                onSuccess={() => {
                                    notification.success({ message: `The provider '${provider.name}' was successfully deleted` })
                                    router.replace("/oidc-providers");
                                }}
                            />

                            <Descriptions column={{ xxl: 3, xl: 3, lg: 2, md: 2, sm: 2, xs: 1 }}>
                                <Descriptions.Item label="Name">{provider.name}</Descriptions.Item>
                                <Descriptions.Item label="Code">{provider.code}</Descriptions.Item>
                                <Descriptions.Item label="Type">{oidcProviderTypesMap[provider.type] || provider.type}</Descriptions.Item>
                                <Descriptions.Item label="Client Id">{provider.clientId}</Descriptions.Item>
                                <Descriptions.Item label="Status">{provider.disabled ? 'Disabled' : 'Enabled'}</Descriptions.Item>
                                <Descriptions.Item label="Created">{localDateTime(provider.created)}</Descriptions.Item>
                                <Descriptions.Item label="Updated">{localDateTime(provider.updated)}</Descriptions.Item>
                            </Descriptions>
                        </Col>
                    </Row>
                }
            >
            </PageContainer >
        </ObjectContainer>
    </>
    )
}