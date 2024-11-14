import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, Card, ConfigProvider, Descriptions, Empty, List, Skeleton, Typography } from "antd";
import { useCallback, useEffect, useState } from "react";
import ApplicationSecretSaveModal from "./application-secret-save-modal";
import ApplicationSecretDeleteModal from "./application-secret-delete-modal";
import { applicationSecretTypesMap } from "@/well-known/applications";
import { localDateTime } from "@/extended/format";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import ApplicationSecretsClient from "@/clients/shoc/identity/application-secrets-client";

export default function ApplicationSecretsCard({ applicationId, loading = false}: any) {

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [creatingActive, setCreatingActive] = useState(false);
    const [editingItem, setEditingItem] = useState(null);
    const [deletingItem, setDeletingItem] = useState(null);

    const load = useCallback(async (id: string) => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(ApplicationSecretsClient).getAll(token, id));

        setProgress(false);

        if (result.error) {
            return;
        }

        setItems(result.payload)
    }, [withToken])

    useEffect(() => {
        if (!applicationId) {
            return;
        }
        load(applicationId);
    }, [load, applicationId]);

    return <>
        <ApplicationSecretSaveModal
            key="add-modal"
            applicationId={applicationId}
            onClose={() => setCreatingActive(false)}
            open={creatingActive}
            onSuccess={() => {
                load(applicationId);
            }}
        />
        <ApplicationSecretSaveModal
            key="edit-modal"
            applicationId={applicationId}
            existing={editingItem}
            edit
            onClose={() => setEditingItem(null)}
            open={editingItem}
            onSuccess={() => {
                load(applicationId);
            }}
        />
        <ApplicationSecretDeleteModal
            key="delete-modal"
            applicationId={applicationId}
            existing={deletingItem}
            onClose={() => setDeletingItem(null)}
            open={deletingItem}
            onSuccess={() => {
                load(applicationId);
            }}
        />
        <Card>
            <Skeleton loading={progress} paragraph={{ rows: 4 }}>

                <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="Secrets" extra={[
                    <Button key="add-button" type="text" size="middle" disabled={progress || loading} icon={<PlusOutlined />} onClick={() => setCreatingActive(true)} />
                ]} />
                <ConfigProvider theme={{ components: { List: { paddingContentHorizontal: 0 } } }} renderEmpty={() => <Empty description={"No secrets to display"}></Empty>}>
                    <List
                        split={false}
                        size="small"
                        bordered={false}
                        loading={progress || loading}
                        dataSource={items}
                        rowKey={item => item.id}
                        renderItem={item => <List.Item key={item.id}
                            actions={[
                                <Button key="edit-button" type="text" size="small" icon={<EditOutlined />} onClick={() => setEditingItem(item)} />,
                                <Button key="delete-button" type="text" size="small" icon={<DeleteOutlined />} danger onClick={() => setDeletingItem(item)} />
                            ]}>
                            <List.Item.Meta
                                title={`Type: ${applicationSecretTypesMap[item.type] || item.type}`}
                                description={<>
                                    <Typography.Paragraph>
                                        Expires: {item.expiration ? localDateTime(item.expiration) : 'No Expiration'}
                                    </Typography.Paragraph>
                                    {item.description}
                                </>}
                            >
                            </List.Item.Meta>
                        </List.Item>
                        } />

            </ConfigProvider>
        </Skeleton>
    </Card >
    </>
}