"use client"

import { ReloadOutlined } from "@ant-design/icons";
import { App, Button, Col, Descriptions, Row, Tabs, Typography } from "antd";
import { useCallback, useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";
import { localDateTime } from "@/extended/format";
import Link from "antd/es/typography/Link";
import PackagesClient from "@/clients/shoc/package/packages-client";
import CodeBlock from "@/components/general/code-block";
import { packageScopesMap } from "@/well-known/packages";

export default function SinglePackageClientPage() {

    const params = useParams();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [item, setItem] = useState<any>({});
    const [fatalError, setFatalError] = useState<any>(null);

    const workspaceId = params?.workspaceId as string || '';
    const packageId = params?.packageId as string || '';

    const load = useCallback(async (workspaceId: string, id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(PackagesClient).getExtendedById(token, workspaceId, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setItem(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        if(workspaceId && packageId){
            load(workspaceId, packageId);
        }
    }, [load, workspaceId, packageId])

    return (
        <>
            <ObjectContainer loading={progress} fatalError={fatalError}>
                <PageContainer fluid title={item.templateReference || "Package Details"}
                    extra={[
                        <Button key="reload" title="Reload" type="default" disabled={progress} onClick={() => load(workspaceId, packageId)}><ReloadOutlined /></Button>,
                    ]}
                    tabProps={{
                        destroyInactiveTabPane: true,
                        items: [
                            {
                                key: "1",
                                label: "Manifest",
                                children: <CodeBlock language="json" code={item.manifest ? JSON.stringify(JSON.parse(item.manifest), null, 4) : ''} />
                            },
                            {
                                key: "2",
                                label: "Containerfile",
                                children: <CodeBlock language="sh" code={item.containerfile} />
                            },
                            {
                                key: "3",
                                label: "Runtime",
                                children: <CodeBlock language="json" code={item.runtime ? JSON.stringify(JSON.parse(item.runtime), null, 4) : ''} />
                            },
                        ]
                    }}
                    tabList={[{ key: "1" }]}
                    content={<>
                        <Row gutter={16}>
                            <Col span={24}>
                                <Descriptions column={2}>
                                    <Descriptions.Item label="Template">{item.templateReference}</Descriptions.Item>
                                    <Descriptions.Item label="Scope">{packageScopesMap[item.scope] || item.scope}</Descriptions.Item>
                                    <Descriptions.Item label="Image"><Typography.Text code copyable>{item.image}</Typography.Text></Descriptions.Item>
                                    <Descriptions.Item label="Checksum"><Typography.Text code copyable>{item.listingChecksum}</Typography.Text></Descriptions.Item>
                                    <Descriptions.Item label="Workspace"><Link href={`/workspaces/${item.workspaceId}`}>{item.workspaceName}</Link></Descriptions.Item>
                                    <Descriptions.Item label="Registry"><Link href={`/registries/${item.registryId}`}>{item.registryName}</Link></Descriptions.Item>
                                    <Descriptions.Item label="Owner"><Link href={`/users/${item.userId}`}>{item.userFullName}</Link></Descriptions.Item>
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
