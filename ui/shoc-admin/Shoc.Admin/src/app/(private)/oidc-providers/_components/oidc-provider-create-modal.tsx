import { selfClient } from "@/clients/shoc";
import OidcProvidersClient from "@/clients/shoc/identity/oidc-providers-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { oidcProviderClientIdMaxLength, oidcProviderClientSecretMaxLength, oidcProviderCodePattern, oidcProviderNameMaxLength, oidcProviderScopeMaxLength, oidcProviderTypes } from "@/well-known/oidc-providers";
import { Form, Input, Modal, Select, Switch } from "antd";
import { useState } from "react";

const formItemLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 6 },
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 18 },
    },
};

export default function OidcProviderCreateModal(props: any) {

    const { visible, onClose, onSuccess } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        form.resetFields();
        setErrors([]);
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(OidcProvidersClient).create(token, {
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

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={visible}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Create Provider"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Unable to create a provider" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout} initialValues={{enabled: false}}>

                <Form.Item name="code" label="Code" rules={[{ required: true, pattern: oidcProviderCodePattern, message: 'Please enter valid code' }]}>
                    <Input placeholder="The code of provider" />
                </Form.Item>

                <Form.Item name="type" label="Type" rules={[{ required: true, message: 'Please select a valid type' }]}>
                    <Select placeholder="Select the type ">
                        {oidcProviderTypes.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>

                <Form.Item name="name" label="Name" rules={[{ required: true, min: 2, max: oidcProviderNameMaxLength, message: 'Please enter valid name' }]}>
                    <Input placeholder="User friendly name for provider" />
                </Form.Item>

                <Form.Item name="authority" label="Authority" rules={[{ required: true, type: 'url', min: 2, message: 'Please enter valid authority' }]}>
                    <Input placeholder="The URL of the provider authority" />
                </Form.Item>

                <Form.Item name="responseType" label="Response Type">
                    <Input placeholder="The expected resonse type (code, id_token, etc)" />
                </Form.Item>

                <Form.Item name="clientId" label="Client Id" rules={[{ required: true, min: 2, max: oidcProviderClientIdMaxLength, message: 'Please enter a valid client id' }]}>
                    <Input placeholder="A client id" />
                </Form.Item>

                <Form.Item name="clientSecret" label="Client Secret" rules={[{ required: true, min: 2, max: oidcProviderClientSecretMaxLength, message: 'Please enter the client secret' }]}>
                    <Input placeholder="A client secret" />
                </Form.Item>

                <Form.Item name="scope" label="Scope" rules={[{ required: true, min: 2, max: oidcProviderScopeMaxLength, message: 'Please enter valid scope' }]}>
                    <Input placeholder="Example: openid email profile" />
                </Form.Item>

                <Form.Item name="fetchUserInfo" label="Fetch Info" valuePropName="checked" rules={[{required: true}]}>
                    <Switch checkedChildren="Yes" unCheckedChildren="No" />
                </Form.Item>

                <Form.Item name="pkce" label="User PKCE" valuePropName="checked" rules={[{required: true}]}>
                    <Switch checkedChildren="Yes" unCheckedChildren="No" />
                </Form.Item>

                <Form.Item name="disabled" label="Status" valuePropName="checked" rules={[{required: true}]}>
                    <Switch checkedChildren="Disabled" unCheckedChildren="Enabled" />
                </Form.Item>

                <Form.Item name="trusted" label="Trusted" valuePropName="checked" rules={[{required: true}]}>
                    <Switch checkedChildren="Yes" unCheckedChildren="No" />
                </Form.Item>

            </Form>
        </Modal>
    );
}
