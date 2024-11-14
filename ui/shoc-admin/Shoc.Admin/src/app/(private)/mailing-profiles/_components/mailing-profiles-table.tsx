"use client"

import { PlusOutlined, RightOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Table } from "antd";
import { localDateTime } from "@/extended/format";
import { useCallback, useEffect, useMemo, useState } from "react";
import MailingProfileCreateModal from "./mailing-profile-create-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import Link from "next/link";
import { selfClient } from "@/clients/shoc";
import MailingProfilesClient from "@/clients/shoc/settings/mailing-profiles-client";
import TableContainer from "@/components/general/table-container";


export default function MailingProfilesTable() {
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [data, setData] = useState<any[]>([]);
    const [errors, setErrors] = useState([]);
    const [creatingActive, setCreatingActive] = useState(false)

    const columns = useMemo(() => [

        {
            title: "Code",
            key: "code",
            dataIndex: "code",
            ellipsis: true
        },
        {
            title: "Provider",
            key: "provider",
            dataIndex: "provider",
            ellipsis: true
        },
        {
            title: "Sender Email",
            key: "defaultFromEmail",
            dataIndex: "defaultFromEmail",
            ellipsis: true
        },
        {
            title: "Sender Name",
            key: "defaultFromSender",
            dataIndex: "defaultFromSender",
            ellipsis: true
        },
        {
            title: "Created",
            key: "created",
            dataIndex: "created",
            render: localDateTime
        },
        {
            title: "Updated",
            key: "updated",
            dataIndex: "updated",
            render: localDateTime
        },
        {
            title: 'More',
            dataIndex: 'id',
            key: 'id',
            render: id => <Link href={`/mailing-profiles/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ], []);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(MailingProfilesClient).getAll(token));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setData(result.payload)
    }, [withToken])

    useEffect(() => {
        load();
    }, [load]);

    return <>
        <MailingProfileCreateModal open={creatingActive} onClose={() => setCreatingActive(false)} onSuccess={() => load()} />

        <Row gutter={0} justify="end" align="middle" style={{ marginBottom: 8 }}>
            <Button icon={<PlusOutlined />} onClick={() => setCreatingActive(true)}>Create</Button>
        </Row>
        <ConfigProvider renderEmpty={() => <Empty description={"No profiles to display"}></Empty>}>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={data}
                    scroll={{ x: true }}
                    loading={progress} />
            </TableContainer>
        </ConfigProvider>
    </>
};