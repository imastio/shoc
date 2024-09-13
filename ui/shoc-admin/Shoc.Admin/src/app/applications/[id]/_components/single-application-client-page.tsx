"use client"

import { EditOutlined, ReloadOutlined } from "@ant-design/icons";
import { Button, Col, Descriptions, Row, Tag } from "antd";
import React, { useCallback, useEffect, useState } from "react";
import ApplicationExtendedTab from "./application-extended-tab";
import { useParams } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import ApplicationsClient from "@/clients/shoc/identity/applications-client";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";
import ApplicationDetailsTab from "./application-details-tab";
import { localDateTime } from "@/extended/format";

export default function SingleApplicationClientPage() {

    const params = useParams();
    const { withToken } = useApiAuthentication();
    const [application, setApplication] = useState<any>({});
    const [updateActive, setUpdateActive] = useState(false);
    const [progress, setProgress] = useState(true);
    const [fatalError, setFatalError] = useState<any>(null);

    const applicationId = params?.id as string || '';

    const load = useCallback(async (id: string) => {

        setProgress(true);
        const result = await withToken((token: string) => selfClient(ApplicationsClient).getById(token, id))
        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setApplication(result.payload || {});

    }, [withToken])

    useEffect(() => {
        if (applicationId) {
            load(applicationId);
        }
    }, [applicationId, load]);

    return (<>
        <ObjectContainer loading={progress} fatalError={fatalError}>
            <PageContainer fluid
                title={application.name || application.applicationClientId || ''}
                extra={[
                    <Button key="update" disabled={progress} onClick={() => setUpdateActive(true)}><EditOutlined /></Button>,
                    <Button key="refresh" disabled={progress} onClick={() => load(applicationId)}><ReloadOutlined /></Button>
                ]}
                tags={[
                    <Tag key="state" color={application.enabled ? 'success' : 'default'}>
                        {application.enabled ? 'Enabled' : 'Disabled'}
                    </Tag>
                ]}
                tabProps={{
                    destroyInactiveTabPane: true,
                    items: [
                        {
                            key: "details",
                            label: "Details",
                            children: <ApplicationDetailsTab application={application} loading={progress} onUpdate={(id: string) => load(id)} />
                        },
                        {
                            key: "extended",
                            label: "Extended",
                            children: <ApplicationExtendedTab application={application} loading={progress} onUpdate={(id: string) => load(id)} />
                        }
                    ]
                }}
                tabList={[
                    {
                        tabKey: "details"
                    },
                    {
                        tabKey: "extended"
                    }
                ]}
                content={
                    <Row gutter={16} align="middle" >
                    <Col lg={24} md={24}>
                        <Descriptions column={{ xxl: 3, xl: 3, lg: 2, md: 2, sm: 2, xs: 1 }}>
                            <Descriptions.Item label="Name">{application.name}</Descriptions.Item>
                            <Descriptions.Item label="Client Id">{application.applicationClientId}</Descriptions.Item>
                            <Descriptions.Item label="Enabled">{application.enabled ? 'Enabled' : 'Disabled'}</Descriptions.Item>
                            <Descriptions.Item label="Description">{application.description}</Descriptions.Item>
                            <Descriptions.Item label="Created">{localDateTime(application.created)}</Descriptions.Item>
                            <Descriptions.Item label="Updated">{localDateTime(application.updated)}</Descriptions.Item>
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