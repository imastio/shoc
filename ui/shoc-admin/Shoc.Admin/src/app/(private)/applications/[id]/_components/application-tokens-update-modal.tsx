import { Form, InputNumber, Modal, Select } from "antd";
import { useEffect, useState } from "react";
import { tokenExpirations, tokenUsageTypes } from "@/well-known/applications";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import ApplicationsClient from "@/clients/shoc/identity/applications-client";
import StandardAlert from "@/components/general/standard-alert";

const formItemLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 12 },
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 12 },
    },
};

export default function ApplicationTokensUpdateModel(props: any) {

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

        if(!open){
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
            title="Update Application: Tokens"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update the application" errors={errors} optional={true} />
            <Form form={form} preserve={true} onFinish={onSubmit} layout="horizontal" {...formItemLayout}>
                <Form.Item name="accessTokenLifetime" label="Access Token Lifetime" rules={[{ required: true }]}>
                    <InputNumber style={{ width: '100%' }} placeholder="3600" />
                </Form.Item>
                <Form.Item name="absoluteRefreshTokenLifetime" label="Abs. Refresh Token Lifetime" rules={[{ required: true }]}>
                    <InputNumber style={{ width: '100%' }} placeholder="2592000" />
                </Form.Item>
                <Form.Item name="slidingRefreshTokenLifetime" label="Sliding Refresh Token Lifetime" rules={[{ required: true }]}>
                    <InputNumber style={{ width: '100%' }} placeholder="1296000" />
                </Form.Item>
                <Form.Item name="refreshTokenUsage" label="Refresh Token Usage" rules={[{ required: true }]}>
                    <Select placeholder="Select the refresh token usage">
                        {tokenUsageTypes.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
                <Form.Item name="refreshTokenExpiration" label="Refresh Token Expiration" rules={[{ required: true }]}>
                    <Select placeholder="Select the refresh token expiration">
                        {tokenExpirations.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
            </Form>
        </Modal>
    );
}
