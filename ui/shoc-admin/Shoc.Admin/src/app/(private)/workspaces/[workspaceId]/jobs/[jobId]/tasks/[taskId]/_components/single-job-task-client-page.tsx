"use client"

import { ReloadOutlined } from "@ant-design/icons";
import { Button, Col, Descriptions, Row } from "antd";
import { useCallback, useEffect, useState } from "react";
import { useParams } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";
import { localDateTime } from "@/extended/format";
import Link from "antd/es/typography/Link";
import CodeBlock from "@/components/general/code-block";
import { jobTaskTypesMap } from "@/well-known/jobs";
import JobTasksClient from "@/clients/shoc/job/job-tasks-client";
import JobTaskStatus from "@/components/job/job-task-status";
import LogsArea from "./logs-area";

export default function SingleJobTaskClientPage() {

    const params = useParams();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [item, setItem] = useState<any>({});
    const [fatalError, setFatalError] = useState<any>(null);

    const workspaceId = params?.workspaceId as string || '';
    const jobId = params?.jobId as string || '';
    const taskId = params?.taskId as string || '';

    const load = useCallback(async (workspaceId: string, jobId: string, id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(JobTasksClient).getExtendedById(token, workspaceId, jobId, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setItem(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        if(workspaceId && jobId && taskId){
            load(workspaceId, jobId, taskId);
        }
    }, [load, workspaceId, jobId, taskId])

    return (
        <>
            <ObjectContainer loading={progress} fatalError={fatalError}>
                <PageContainer fluid title={Number.isSafeInteger(item.sequence) ? `Task ${item.sequence}` : "Job Task Details"}
                    tags={<JobTaskStatus status={item.status} />} 
                    extra={[
                        <Button key="reload" title="Reload" type="default" disabled={progress} onClick={() => load(workspaceId, jobId, taskId)}><ReloadOutlined /></Button>,
                    ]}
                    tabProps={{
                        destroyInactiveTabPane: true,
                        items: [
                            {
                                key: "0",
                                label: "Spec",
                                children: <CodeBlock language="json" code={item.spec ? JSON.stringify(JSON.parse(item.spec), null, 4) : ''} />
                            },
                            {
                                key: "1",
                                label: "Runtime",
                                children: <CodeBlock language="json" code={item.runtime ? JSON.stringify(JSON.parse(item.runtime), null, 4) : ''} />
                            },
                            {
                                key: "2",
                                label: "Logs",
                                children: <LogsArea workspaceId={workspaceId} jobId={jobId} taskId={taskId} />
                            },
                        ]
                    }}
                    tabList={[]}
                    content={<>
                        <Row gutter={16}>
                            <Col span={24}>
                                <Descriptions column={2}>
                                    <Descriptions.Item label="Identity">{item.sequence}</Descriptions.Item>
                                    <Descriptions.Item label="Type">{jobTaskTypesMap[item.type] || item.type}</Descriptions.Item>
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
