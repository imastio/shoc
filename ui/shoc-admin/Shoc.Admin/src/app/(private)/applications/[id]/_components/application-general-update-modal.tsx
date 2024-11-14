import { selfClient } from "@/clients/shoc";
import ApplicationsClient from "@/clients/shoc/identity/applications-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal, Switch } from "antd";
import { useEffect, useState } from "react";

const formItemLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 8 },
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 16 },
    },
};

export default function ApplicationGeneralUpdateModal(props: any) {

    const { open, onClose = () => { }, onSuccess, application } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        setErrors([]);
        onClose()
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(ApplicationsClient).updateById(token, application.id, {
            ...application,
            ...values
        }));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        if (onSuccess) {
            onSuccess(result.payload);
        }

        onCloseWrapper();
    }

    useEffect(() => {

        if (!open) {
            return;
        }

        form.setFieldsValue({
            ...application
        });

    }, [form, application, open])

    return (
        <Modal
            closable={false}
            forceRender
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Update Application: General"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update the application" errors={errors} optional={true} />
            <Form form={form} preserve={true} onFinish={onSubmit} layout="horizontal" {...formItemLayout}>
                <Form.Item name="allowedScopes" label="Allowed Scopes">
                    <Input placeholder="openid email profile" />
                </Form.Item>
                <Form.Item name="allowedGrantTypes" label="Allowed Grant Types">
                    <Input placeholder="authorization_code client_credentials" />
                </Form.Item>
                <Form.Item name="secretRequired" label="Secret Required" valuePropName="checked">
                    <Switch checkedChildren="Yes" unCheckedChildren="No" />
                </Form.Item>
                <Form.Item name="pkceRequired" label="PKCE Required" valuePropName="checked">
                    <Switch checkedChildren="Yes" unCheckedChildren="No" />
                </Form.Item>
                <Form.Item name="allowOfflineAccess" label="Allow Offline Access" valuePropName="checked">
                    <Switch checkedChildren="Yes" unCheckedChildren="No" />
                </Form.Item>

            </Form>
        </Modal>
    );
}
