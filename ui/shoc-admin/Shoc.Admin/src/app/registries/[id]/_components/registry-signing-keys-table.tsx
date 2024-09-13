import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, ConfigProvider, Empty, Row, Space, Table } from "antd";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import { localDateTime } from "@/extended/format";
import TableContainer from "@/components/general/table-container";
import { registrySigningKeyAlgorithmsMap, registrySigningKeyUsagesMap } from "@/well-known/registries";
import RegistrySigningKeysClient from "@/clients/shoc/registry/registry-signing-keys-client";
import RegistrySigningKeysAddModal from "./registry-signing-keys-add-modal";
import RegistrySigningKeysDeleteModal from "./registry-signing-keys-delete-modal";

export function RegistrySigningKeysTable({registryId, loading, onUpdate = () => {}}: any){

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [errors, setErrors] = useState<any[]>([]);
    const [addingActive, setAddingActive] = useState(false);
    const [deletingItem, setDeletingItem] = useState(null);

    const columns = useMemo(() => [
        {
            title: "Key ID",
            key: "keyId",
            dataIndex: "keyId",
            ellipsis: true
        },
        {
            title: "Algorithm",
            key: "algorithm",
            dataIndex: "algorithm",
            ellipsis: true,
            render: (algorithm: string) => registrySigningKeyAlgorithmsMap[algorithm]
        },
        {
            title: "Usage",
            key: "usage",
            dataIndex: "usage",
            render: (usage: string) => registrySigningKeyUsagesMap[usage] 
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
            render: (_: string, item: any) => <Space>
                <Button key="delete-button" size="small" icon={<DeleteOutlined />} danger  onClick={() => setDeletingItem(item)} />
            </Space>
        }
    ], []);

    const load = useCallback(async (id: string) => {
        setProgress(true);
        setErrors([]);

        const result = await withToken((token: string) => selfClient(RegistrySigningKeysClient).getAll(token, id));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setItems(result.payload)
    }, [withToken])

    useEffect(() => {
        if (!registryId) {
            return;
        }
        load(registryId);
    }, [load, registryId]);

    return <>
        <RegistrySigningKeysAddModal
            key="add-modal"
            registryId={registryId}
            onClose={() => setAddingActive(false)}
            open={addingActive}
            onSuccess={(entity: any) => {
                load(registryId);
                onUpdate(entity);
            }}
        />
        <RegistrySigningKeysDeleteModal
            key="delete-modal"
            registryId={registryId}
            existing={deletingItem}
            onClose={() => setDeletingItem(null)}
            open={deletingItem}
            onSuccess={(entity: any) => {
                load(registryId);
                onUpdate(entity);
            }}
        />
        <ConfigProvider renderEmpty={() => <Empty description={"No signing keys to display"}></Empty>}>
            <Row gutter={0} justify="end" align="middle" style={{ margin: '8px 0' }}>
                <Space size="small">
                <Button key="add-button" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setAddingActive(true)}>
                    Add key
                </Button>
                </Space>
            </Row>
            <TableContainer errors={errors}>
                <Table
                    rowKey={record => record.id}
                    columns={columns}
                    dataSource={items}
                    loading={progress}
                    scroll={{ x: true }}
                />
            </TableContainer>
        </ConfigProvider>
    </>
}