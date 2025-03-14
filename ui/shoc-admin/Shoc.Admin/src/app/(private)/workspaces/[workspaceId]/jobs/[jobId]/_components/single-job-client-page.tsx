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
import CodeBlock from "@/components/general/code-block";
import { packageScopesMap } from "@/well-known/packages";
import JobsClient from "@/clients/shoc/job/jobs-client";
import { jobScopesMap } from "@/well-known/jobs";
import JobStatus from "@/components/job/job-status";

export default function SingleJobClientPage() {

    const params = useParams();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [item, setItem] = useState<any>({});
    const [fatalError, setFatalError] = useState<any>(null);

    const workspaceId = params?.workspaceId as string || '';
    const jobId = params?.jobId as string || '';

    const load = useCallback(async (workspaceId: string, id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(JobsClient).getExtendedById(token, workspaceId, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setItem(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        if(workspaceId && jobId){
            load(workspaceId, jobId);
        }
    }, [load, workspaceId, jobId])

    return (
        <>
            <ObjectContainer loading={progress} fatalError={fatalError}>
                <PageContainer fluid title={item.localId ? `Job ${item.localId}` : "Job Details"}
                    tags={<JobStatus status={item.status} />} 
                    extra={[
                        <Button key="reload" title="Reload" type="default" disabled={progress} onClick={() => load(workspaceId, jobId)}><ReloadOutlined /></Button>,
                    ]}
                    tabProps={{
                        destroyInactiveTabPane: true,
                        items: [
                            {
                                key: "1",
                                label: "Manifest",
                                children: <CodeBlock language="json" code={item.manifest ? JSON.stringify(JSON.parse(item.manifest), null, 4) : ''} />
                            }
                        ]
                    }}
                    tabList={[{ key: "1" }]}
                    content={<>
                        <Row gutter={16}>
                            <Col span={24}>
                                <Descriptions column={2}>
                                    <Descriptions.Item label="Identity">{item.localId}</Descriptions.Item>
                                    <Descriptions.Item label="Scope">{jobScopesMap[item.scope] || item.scope}</Descriptions.Item>
                                    <Descriptions.Item label="Cluster"><Link href={`/workspaces/${item.workspaceId}/clusters/${item.clusterId}`}>{item.clusterName}</Link></Descriptions.Item>
                                    <Descriptions.Item label="Workspace"><Link href={`/workspaces/${item.workspaceId}`}>{item.workspaceName}</Link></Descriptions.Item>
                                    <Descriptions.Item label="Owner"><Link href={`/users/${item.userId}`}>{item.userFullName}</Link></Descriptions.Item>
                                    <Descriptions.Item label="Pending at">{localDateTime(item.pendingAt)}</Descriptions.Item>
                                    <Descriptions.Item label="Running at">{localDateTime(item.runningAt)}</Descriptions.Item>
                                    <Descriptions.Item label="Completed at">{localDateTime(item.completedAt)}</Descriptions.Item>
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
