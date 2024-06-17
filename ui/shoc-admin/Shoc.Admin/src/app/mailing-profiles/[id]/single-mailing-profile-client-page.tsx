"use client"

import { DownOutlined } from "@ant-design/icons";
import { App, Button, Col, Descriptions, Dropdown, Row, Spin } from "antd";
import MailingProfileDeleteModal from "./_components/mailing-profile-delete-modal";
import MailingProfileUpdateApiSecretModal from "./_components/mailing-profile-update-api-secret-modal";
import MailingProfileUpdatePasswordModal from "./_components/mailing-profile-update-password-modal";
import { localDateTime } from "@/extended/format";
import { useCallback, useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import MailingProfilesClient from "@/clients/shoc/settings/mailing-profiles-client";
import { ObjectContainer } from "@/components/general/object-container";
import PageContainer from "@/components/general/page-container";

export default function SingleMailingProfileClientPage() {

    const params = useParams();
    const { withToken } = useApiAuthentication();
    const router = useRouter();
    const [progress, setProgress] = useState(true);
    const [profile, setProfile] = useState<any>({});
    const [editPasswordActive, setEditPasswordActive] = useState(false);
    const [editApiSecretActive, setEditApiSecretActive] = useState(false);
    const [deleteActive, setDeleteActive] = useState(false);
    const [fatalError, setFatalError] = useState<any>(null);
    const { notification } = App.useApp();

    const profileId = params?.id as string || '';

    const actionMenuItems = [
        {
            key: "edit-password",
            label: "Edit Password",
            disabled: progress || !profile.id,
        },
        {
            key: "edit-api-secret",
            label: "Edit API Secret",
            disabled: progress || !profile.id,
        },
        {
            key: "delete",
            label: "Delete",
            disabled: progress || !profile.id,
            danger: true
        }
    ]

    const menuClickHandler = ({ key }: { key: string }) => {
        switch (key) {
            case "edit-password":
                setEditPasswordActive(true);
                break;
            case "edit-api-secret":
                setEditApiSecretActive(true);
                break;
            case "delete":
                setDeleteActive(true);
                break;
            default:
                return;
        }
    }

    const load = useCallback(async (id: string) => {

        setProgress(true);

        const result = await withToken((token: string) => selfClient(MailingProfilesClient).getById(token, id));

        setProgress(false);

        if (result.error) {
            setFatalError({ statusCode: result.error.response.status, errors: [...result.payload.errors] })
            return;
        }

        setProfile(result.payload || {});
    }, [withToken]);

    useEffect(() => {
        if (profileId) {
            load(profileId);
        }
    }, [load, profileId])

    return (
        <>
            <ObjectContainer loading={progress} fatalError={fatalError}>
                <PageContainer fluid title={profile.code ? `Profile: ${profile.code}` : "Mailing Profile"}
                    extra={[
                        <Dropdown key="1" menu={{ items: actionMenuItems, onClick: menuClickHandler }} >
                            <Button type="default">Actions <DownOutlined /> </Button>
                        </Dropdown>
                    ]}
                    content={
                        <>
                            <MailingProfileUpdatePasswordModal
                                open={editPasswordActive}
                                profileId={profile.id}
                                onClose={() => setEditPasswordActive(false)}
                                onSuccess={() => load(profileId)}
                            />
                            <MailingProfileUpdateApiSecretModal
                                open={editApiSecretActive}
                                profileId={profile.id}
                                onClose={() => setEditApiSecretActive(false)}
                                onSuccess={() => load(profileId)}
                            />
                            <MailingProfileDeleteModal
                                visible={deleteActive}
                                profileId={profile.id}
                                onClose={() => setDeleteActive(false)}
                                onSuccess={() => {
                                    notification.success({ message: `The profile '${profile.code}' was successfully deleted` })
                                    router.replace("/mailing-profiles");
                                }}
                            />
                            {profile.id && <Row gutter={32} style={{ marginTop: 16 }}>
                                <Col span={24}>
                                    <Spin spinning={progress}>
                                        <Descriptions layout="horizontal" bordered>
                                            <Descriptions.Item label="Code">{profile.code || "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="Provider">{profile.provider || "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="Server">{profile.server || "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="Port">{profile.port || "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="Encryption Type">{profile.encryptionType || "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="Username">{profile.username || "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="Password">{profile.passwordEncrypted ? "Encrypted Value" : "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="API URL">{profile.apiUrl || "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="Application ID">{profile.applicationId || "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="API Secret">{profile.apiSecretEncrypted ? "Encrypted Value" : "N/A"}</Descriptions.Item>
                                            <Descriptions.Item label="Creation Time">{localDateTime(profile.created)}</Descriptions.Item>
                                            <Descriptions.Item label="Update Time">{localDateTime(profile.updated)}</Descriptions.Item>
                                        </Descriptions>
                                    </Spin>
                                </Col>
                            </Row>
                            }
                        </>
                    }
                >
            </PageContainer>
        </ObjectContainer>
        </>
    )
};