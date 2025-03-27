"use client"
import { PlusOutlined, ReloadOutlined, RightOutlined } from "@ant-design/icons";
import { Button, Row, Space, Table } from "antd";
import { useCallback, useEffect, useState } from "react";
import OidcProviderCreateModal from "./oidc-provider-create-modal";
import Link from "next/link";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import TableContainer from "@/components/general/table-container";
import OidcProvidersClient from "@/clients/shoc/identity/oidc-providers-client";
import { oidcProviderTypesMap } from "@/well-known/oidc-providers";

export default function OidcProvidersTable() {

    const columns = [
        {
            title: 'Code',
            dataIndex: 'code',
            key: 'code',
            ellipsis: true
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
            ellipsis: true
        },
        {
            title: 'Type',
            dataIndex: 'type',
            key: 'type',
            render: (type: string) => oidcProviderTypesMap[type] || type
        },
        {
            title: 'Authority',
            dataIndex: 'authority',
            key: 'authority',
            ellipsis: true
        },
        {
            title: 'Response Type',
            dataIndex: 'responseType',
            key: 'responseType',
            ellipsis: true
        },
        {
            title: 'Client Id',
            dataIndex: 'clientId',
            key: 'clientId',
            ellipsis: true
        },
        {
            title: 'Scope',
            dataIndex: 'scope',
            key: 'scope',
            ellipsis: true
        },
        {
            title: 'Fetch User Info',
            dataIndex: 'fetchUserInfo',
            key: 'fetchUserInfo',
            render: (fetchUserInfo: boolean) => fetchUserInfo ? 'Yes' : 'No',
        },
        {
            title: 'Status',
            dataIndex: 'disabled',
            key: 'disabled',
            render: (disabled: boolean) => disabled ? 'Disabled' : 'Enabled',
            ellipsis: true
        },
        {
            title: 'Pkce',
            dataIndex: 'pkce',
            key: 'pkce',
            render: (pkce: boolean) => pkce ? 'Yes' : 'No',
            ellipsis: true
        },
        {
            title: 'Trusted',
            dataIndex: 'trusted',
            key: 'trusted',
            render: (trusted: boolean) => trusted ? 'Yes' : 'No',
            ellipsis: true
        },
        {
            title: 'More',
            dataIndex: 'id',
            key: 'id',
            width: '100px',
            render: (id: string) => <Link href={`/oidc-providers/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ];

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [data, setData] = useState([]);
    const [errors, setErrors] = useState([]);
    const [creatingActive, setCreatingActive] = useState(false)

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(OidcProvidersClient).getAll(token));

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
                <OidcProviderCreateModal
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
