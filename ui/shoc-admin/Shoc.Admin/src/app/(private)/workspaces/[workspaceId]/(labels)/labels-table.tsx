import { DeleteOutlined, PlusOutlined, ReloadOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Input, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { localDateTime } from "@/extended/format";
import TableContainer from "@/components/general/table-container";
import LabelsClient from "@/clients/shoc/job/labels-client";
import LabelsCreateModal from "./labels-create-modal";
import LabelsDeleteModal from "./labels-delete-modal";

export function LabelsTable({ workspaceId, loading, onUpdate = () => { } }: any) {

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [errors, setErrors] = useState<any[]>([]);
    const [search, setSearch] = useState<string>('');
    const [addingActive, setAddingActive] = useState(false);
    const [deletingItem, setDeletingItem] = useState(null);

    const filteredItems = useMemo(() => {
        return items.filter(item => (item.name as string ?? '').toLowerCase().includes(search.toLowerCase()))
    }, [items, search])

    const columns = useMemo(() => [
        {
            title: "Name",
            key: "name",
            dataIndex: "name",
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
            title: "Actions",
            key: "actions",
            dataIndex: "id",
            width: '150px',
            render: (_: string, item: any) => <Space>
                <Button key="delete-button" size="small" icon={<DeleteOutlined />} danger onClick={() => setDeletingItem(item)} />
            </Space>
        }
    ], []);

    const load = useCallback(async (id: string) => {
        setProgress(true);
        setErrors([]);

        const result = await withToken((token: string) => selfClient(LabelsClient).getAll(token, id));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setItems(result.payload)
    }, [withToken])

    useEffect(() => {
        if (!workspaceId) {
            return;
        }
        load(workspaceId);
    }, [load, workspaceId]);

    return <>
        <LabelsCreateModal
            key="add-modal"
            workspaceId={workspaceId}
            onClose={() => setAddingActive(false)}
            open={addingActive}
            onSuccess={(entity: any) => {
                load(workspaceId);
                onUpdate(entity);
            }}
        />
        <LabelsDeleteModal
            key="delete-modal"
            workspaceId={workspaceId}
            existing={deletingItem}
            onClose={() => setDeletingItem(null)}
            open={deletingItem}
            onSuccess={(entity: any) => {
                load(workspaceId);
                onUpdate(entity);
            }}
        />
        <ConfigProvider renderEmpty={() => <Empty description={"No labels to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                    <Input key="search-box" placeholder="Type something to search..." disabled={progress || loading} value={search} onChange={(e) => setSearch(e.target.value)} />
                    <Button key="refresh" size="middle" disabled={progress || loading} icon={<ReloadOutlined />} onClick={() => load(workspaceId)} />
                    <Button key="add-button" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)}>
                        New label
                    </Button>
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={filteredItems}
                    loading={progress}
                    scroll={{ x: true }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
}