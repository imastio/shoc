import { ReloadOutlined, RightOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { localDateTime } from "@/extended/format";
import TableContainer from "@/components/general/table-container";
import Link from "next/link";
import { packageScopesMap } from "@/well-known/packages";
import PackagesClient from "@/clients/shoc/package/packages-client";

export function PackagesTable({ workspaceId, loading, onUpdate = () => { } }: any) {

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [paging, setPaging] = useState({ page: 0, size: 5 });
    const [items, setItems] = useState({ items: [], totalCount: 0 });
    const [errors, setErrors] = useState<any[]>([]);
    
    const columns = useMemo(() => [
        {
            title: "Template",
            key: "templateReference",
            dataIndex: "templateReference",
            ellipsis: true
        },
        {
            title: "Scope",
            key: "scope",
            dataIndex: "scope",
            render: (scope: string) => packageScopesMap[scope] || scope 
        },
        {
            title: "Owner",
            key: "userId",
            dataIndex: "userId",
            render: (userId: string, row: any) => <Link prefetch={false} href={`/users/${userId}`}>{row.userFullName}</Link>
        },
        {
            title: "Registry",
            key: "registryId",
            dataIndex: "registryId",
            render: (registryId: string, row: any) => <Link prefetch={false} href={`/registries/${registryId}`}>{row.registryName}</Link>
        },
        {
            title: "Created",
            key: "created",
            dataIndex: "created",
            ellipsis: true,
            width: '200px',
            render: localDateTime
        },
        {
            title: "Updated",
            key: "updated",
            dataIndex: "updated",
            ellipsis: true,
            width: '200px',
            render: localDateTime
        },
        {
            title: 'More',
            dataIndex: 'id',
            key: 'id',
            width: '100px',
            render: (id: string, row: any) => <Link prefetch={false} href={`/workspaces/${row.workspaceId}/packages/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ], []);

    const load = useCallback(async (id: string) => {
        setProgress(true);
        setErrors([]);

        const result = await withToken((token: string) => selfClient(PackagesClient).getExtendedPage(token, id, {}, paging.page, paging.size ));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setItems(result.payload)
    }, [withToken, paging])

    useEffect(() => {
        if (!workspaceId) {
            return;
        }
        load(workspaceId);
    }, [load, workspaceId]);

    return <>
        <ConfigProvider renderEmpty={() => <Empty description={"No packages to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                    <Button key="refresh" size="middle" disabled={progress || loading} icon={<ReloadOutlined />} onClick={() => load(workspaceId)} />
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={items.items}
                    loading={progress}
                    scroll={{ x: true }}
                    pagination={{
                        onChange: (page, size) => setPaging({ page: page - 1, size }),
                        showTotal: () => false,
                        total: items.totalCount,
                        hideOnSinglePage: false,
                        showPrevNextJumpers: true,
                        showLessItems: false,
                        defaultPageSize: paging.size,
                        pageSize: paging.size
                    }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
}