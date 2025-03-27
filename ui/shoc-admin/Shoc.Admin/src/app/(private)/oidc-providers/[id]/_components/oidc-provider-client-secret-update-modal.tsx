import { selfClient } from "@/clients/shoc";
import OidcProvidersClient from "@/clients/shoc/identity/oidc-providers-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { oidcProviderClientSecretMaxLength } from "@/well-known/oidc-providers";
import { Form, Input, Modal } from "antd";
import { useEffect, useState } from "react";

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

export default function OidcProviderClientSecretUpdateModal(props: any) {

    const { open, onClose = () => { }, onSuccess, provider } = props;

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

        const result = await withToken((token: any) => selfClient(OidcProvidersClient).updateClientSecretById(token, provider.id, {
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
            clientSecret: ''
        });

    }, [form, open])

    return (
        <Modal
            closable={false}
            forceRender
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Update Provider Client Secret"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update the provider's client secret" errors={errors} optional={true} />
            <Form form={form} preserve={true} onFinish={onSubmit} layout="horizontal" {...formItemLayout}>

                <Form.Item name="clientSecret" label="Client Secret" rules={[{ required: true, min: 2, max: oidcProviderClientSecretMaxLength, message: 'Please enter the client secret' }]}>
                    <Input placeholder="A client secret" />
                </Form.Item>

            </Form>
        </Modal>
    );
}
