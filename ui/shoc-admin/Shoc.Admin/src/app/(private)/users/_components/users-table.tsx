"use client"

import { RightOutlined, PlusOutlined, SearchOutlined } from "@ant-design/icons";
import { Button, Table, Tag, Row, Segmented, Input, Col } from "antd";
import { useCallback, useEffect, useState } from "react";
import { userTypes } from "@/well-known/user-types";
import UserCreateModal from "./user-create-modal";
import useDebounce from "@/hooks/useDebounce";
import { localDateTime } from "@/extended/format";
import Link from "next/link";
import TableContainer from "@/components/general/table-container";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";

export default function UsersTable() {

    const columns = [
        {
            title: 'Full Name',
            dataIndex: 'fullName',
            key: 'fullName',
        },
        {
            title: 'Email',
            dataIndex: 'email',
            key: 'email',
        },
        {
            title: 'Verified',
            dataIndex: ['emailVerified', 'phoneVerified'],
            key: 'verified',
            render: (_: any, user: any) => (user.emailVerified || user.phoneVerified) ? <Tag color="success">Verified</Tag> : <Tag color="warning">Not verified</Tag>
        },
        {
            title: 'Username',
            dataIndex: 'username',
            key: 'username',
        },
        {
            title: 'Timezone',
            dataIndex: 'timezone',
            key: 'timezone',
        },
        {
            title: 'Logged In',
            dataIndex: 'lastLogin',
            key: 'lastLogin',
            render: (lastLogin: string) => localDateTime(lastLogin)
        },
        {
            title: 'More',
            dataIndex: 'id',
            key: 'id',
            render: (id: string) => <Link href={`/users/${id}`}><Button><RightOutlined /></Button></Link>
        }
    ];

    const { withToken } = useApiAuthentication();
    const [paging, setPaging] = useState({ page: 0, size: 10 });
    const [progress, setProgress] = useState(true);
    const [users, setUsers] = useState({ items: [], totalCount: 0 });
    const [errors, setErrors] = useState([]);
    const [creatingActive, setCreatingActive] = useState(false);
    const [selectedType, setSelectedType] = useState('');
    const [search, setSearch] = useState('');
    const debouncedSearch = useDebounce(search);

    const load = useCallback(async () => {
        setProgress(true);

        const filterInput = {
            search: debouncedSearch,
            type: selectedType
        };

        const result = await withToken((token: string) => selfClient(UsersClient).getAll(token, filterInput, paging.page, paging.size));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }
        setUsers({
            items: result.payload?.items || [],
            totalCount: result.payload?.totalCount || 0
        });
    }, [withToken, paging, selectedType, debouncedSearch])

    useEffect(() => {
        load();
    }, [load]);

    return (
        <>
            <UserCreateModal
                visible={creatingActive}
                onClose={() => setCreatingActive(false)}
                onSuccess={() => load()}
            />
            <Row gutter={8} justify="end" align="middle">
                <Col>
                    <Input prefix={<SearchOutlined />} value={search} onChange={e => setSearch(e.target.value)} placeholder="Search" />

                </Col>
                <Col>
                    <Button icon={<PlusOutlined />} onClick={() => setCreatingActive(true)}>Create</Button>
                </Col>
                <Col>
                    <Segmented style={{ margin: "8px 0 8px 0" }}
                        defaultValue={selectedType}
                        value={selectedType}
                        onChange={setSelectedType} options={[{ label: 'All', value: '' }, ...userTypes.map((kv: any) => ({ label: kv.display, value: kv.key }))]} />
                </Col>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey="id"
                    tableLayout="auto"
                    columns={columns}
                    dataSource={users.items}
                    loading={progress}
                    scroll={{ x: true }}
                    pagination={{
                        onChange: (page, size) => setPaging({ page: page - 1, size }),
                        showTotal: () => false,
                        total: users.totalCount,
                        hideOnSinglePage: false,
                        showPrevNextJumpers: true,
                        showLessItems: false,
                        defaultPageSize: paging.size,
                        pageSize: paging.size
                    }}
                />
            </TableContainer>
        </>
    )
}