"use client"
import { PlusOutlined, ReloadOutlined, RightOutlined } from "@ant-design/icons";
import { Button, Row, Space, Table } from "antd";
import { useCallback, useEffect, useState } from "react";
import ApplicationCreateModal from "./application-create-modal";
import Link from "next/link";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import ApplicationsClient from "@/clients/shoc/identity/applications-client";
import TableContainer from "@/components/general/table-container";

export default function ApplicationsTable() {

    const columns = [
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
            ellipsis: true,
            render: (name: string) => name || 'Not Provided'
        },
        {
            title: 'Description',
            dataIndex: 'description',
            key: 'description',
            ellipsis: true
        },
        {
            title: 'Client Id',
            dataIndex: 'applicationClientId',
            key: 'applicationClientId',
            ellipsis: true
        },
        {
            title: 'Enabled',
            dataIndex: 'enabled',
            key: 'enabled',
            render: (enabled: boolean) => enabled ? 'Enabled' : 'Disabled',
            ellipsis: true
        },
        {
            title: 'More',
            dataIndex: 'id',
            key: 'id',
            width: '100px',
            render: (id: string) => <Link href={`/applications/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ];

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [data, setData] = useState([]);
    const [errors, setErrors] = useState([]);
    const [creatingActive, setCreatingActive] = useState(false)

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(ApplicationsClient).getAll(token));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }
        setData(result.payload);

    }, [withToken])

    useEffect(() => {
        load();
    }, [load]);


    return (
        <>
            <Row gutter={0} justify="end" align="middle" style={{ marginBottom: 8 }}>
                <ApplicationCreateModal
                    visible={creatingActive}
                    onClose={() => setCreatingActive(false)}
                    onSuccess={() => load()} />
                <Space size="small">
                    <Button disabled={progress} icon={<PlusOutlined />} onClick={() => setCreatingActive(true)}>Create</Button>
                    <Button disabled={progress} icon={<ReloadOutlined />} onClick={load} />
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey="id"
                    tableLayout="fixed"
                    showSorterTooltip={false}
                    columns={columns}
                    scroll={{ x: true }}
                    dataSource={data}
                    loading={progress}
                />
            </TableContainer>
        </>
    )
}
