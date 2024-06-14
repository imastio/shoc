"use client"

import { PlusOutlined, RightOutlined, SearchOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Input, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import PrivilegeCreateModal from "./privilege-create-modal";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { localDateTime } from "@/extended/format";
import Link from "next/link";
import { selfClient } from "@/clients/shoc";
import PrivilegesClient from "@/clients/shoc/identity/privileges-client";
import TableContainer from "@/components/general/table-container";

export default function PrivilegesTable() {
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [privileges, setPrivileges] = useState<any[]>([]);
    const [errors, setErrors] = useState([]);
    const [search, setSearch] = useState<string>('');
    const [creatingActive, setCreatingActive] = useState(false)

    const columns = useMemo(() => [
        {
            title: "Name",
            key: "name",
            dataIndex: "name",
            ellipsis: true
        },
        {
            title: "Category",
            key: "category",
            dataIndex: "category",
            ellipsis: true
        },
        {
            title: "Description",
            key: "description",
            dataIndex: "description",
            ellipsis: true
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
            render: (id: string) => <Link href={`/privileges/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ], []);

    var filteredPrivileges = useMemo(() => {
        const searchLower = search?.toLowerCase()

        return search  
        ? privileges.filter((p) => (p.name?.toLowerCase())?.includes(searchLower) || (p.category?.toLowerCase())?.includes(searchLower))
        : privileges
    }, [privileges, search])

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(PrivilegesClient).getAll(token));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setPrivileges(result.payload)
    }, [withToken])

    useEffect(() => {
        load();
    }, [load]);

    return <>
        <PrivilegeCreateModal open={creatingActive} onClose={() => setCreatingActive(false)} onSuccess={(entity: any) => setPrivileges([...privileges, entity])} />
        <ConfigProvider renderEmpty={() => <Empty description={"No privileges to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                    <Input prefix={<SearchOutlined />} value={search} onChange={e => setSearch(e.target.value)} placeholder="Search" />
                    <Button icon={<PlusOutlined />} onClick={() => setCreatingActive(true)}>Create</Button>
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={filteredPrivileges}
                    loading={progress}
                    scroll={{ x: true }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
};